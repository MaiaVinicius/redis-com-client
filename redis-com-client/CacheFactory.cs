using StackExchange.Redis;

namespace redis_com_client
{
    public class CacheFactory
    {
        private static ConnectionMultiplexer _redisClientsManager;


        private CacheFactory()
        {
            
        }

        public static IDatabase GetInstance(string configuration)
        {
            if (_redisClientsManager == null)
                _redisClientsManager = ConnectionMultiplexer.Connect(configuration: configuration);

            return _redisClientsManager.GetDatabase();
        }

        public static IServer GetServer()
        {
            if (_redisClientsManager == null)
                _redisClientsManager = ConnectionMultiplexer.Connect("localhost");

            return _redisClientsManager.GetServer("localhost", 6379);
        }

        public static void Close(bool allowCommandsToComplete = true)
        {
            _redisClientsManager.Close(allowCommandsToComplete);
        }
    }
}
