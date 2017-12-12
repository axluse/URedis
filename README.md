# URedis
ver1.10
Unity ver.2017以降のc# .Net4以上に設定されたプロジェクトのみで使用可能。
非同期対応

基本システム=>TeamDev.Redis

## 使い方

### 1.URedisセットアップ　-Using-
    using URedis;
    using System.Threading.Tasks;

### 2.URedis->Redisサーバー接続
    public async void ExampleConnect() {
      redis = new Redis();
      await redis.Connect(ipAddress, port);
      Debug.Log("Connected");
    }
### 3.データのセット
    private async void ExampleSet() {
      await redis.Set(key, value);
      Debug.Log("Setted");
    }
### 4.データのゲット
    private async void ExampleGet() {
      Task<string> getter = redis.Get(key);
      string data = await getter;
      Debug.Log(data);
    }
