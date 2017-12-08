
#region Include
using System;
using System.Threading.Tasks;
using UnityEngine;
using TeamDev.Redis;
#endregion

namespace Redis {
    public class Redis : MonoBehaviour {

        private static RedisDataAccessProvider client;
        private static bool clientOpenCD = false;

        /// <summary>
        /// Redis Connect
        /// </summary>
        /// <param name="host">IP Address / Domain</param>
        /// <param name="port">Port [Default=6379]</param>
        /// <returns></returns>
        public static async Task<bool> Connect(string host, int port = 6379) {
            client = new RedisDataAccessProvider();
            client.Configuration.Host = host;
            client.Configuration.Port = port;
            await Task.Run(()=> client.Connect());
            clientOpenCD = true;
            return true;
        }

        /// <summary>
        /// key&valueを指定して、redisに対してpub/subを実行します
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>true=正常終了</returns>
        public static async Task<bool> Set(string key, string value) {
            if(clientOpenCD) {
                try {
                    await Task.Run(() => {
                        client.SendCommand(RedisCommand.SET, key, value);
                        client.WaitComplete();
                    });
                    return true;
                } catch (Exception e) {
                    Debug.LogWarning($"[Redis] {e} ");
                    return false;
                }
            } else {
                Debug.LogError("[Redis Error] Redis.Connectを先に実行してください");
                return false;
            }
        }

        /// <summary>
        /// 指定したkeyの値を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <returns>value</returns>
        public static async Task<string> Get(string key) {
            if (clientOpenCD) {
                try {
                    client.SendCommand(RedisCommand.GET, key);
                    string res = "";
                    await Task.Run(() => { res = client.ReadString(); });
                    return res;
                } catch (Exception e) {
                    Debug.LogWarning($"[Redis] {e} ");
                    return "";
                }
            } else {
                Debug.LogError("[Redis Error] Redis.Connectを先に実行してください");
                return "";
            }
        }

        /// <summary>
        /// 接続を安全に終了させます
        /// </summary>
        /// <returns>true=正常終了</returns>
        public static async Task<bool> Disconnect() {
            if (clientOpenCD) {
                await Task.Run(() => client.Close());
                clientOpenCD = false;
                return true;
            } else {
                Debug.LogError("[Redis Error] Redis.Connectを先に実行してください");
                return false;
            }
        }
    }
}