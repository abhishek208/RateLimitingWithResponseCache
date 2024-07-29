using StackExchange.Redis;

namespace RateLimitingWithResponseCache
{
    public class CacheHelper
    {
        private static IDatabase Cache;
        public static void BuildCache()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            Cache = redis.GetDatabase();
        }

        public static void StringSet(string cachekey, string value, TimeSpan ttl)
        {
            try { Cache.StringSet(cachekey, value, ttl); } catch { }
        }

        public static void StringSet(string cachekey, string value)
        {
            try { Cache.StringSet(cachekey, value); } catch { }
        }

        public static string StringGet(string cachekey)
        {
            try { return Cache.StringGet(cachekey); } catch { return ""; }
        }

        public static bool KeyExists(string cachekey)
        {
            try { return Cache.KeyExists(cachekey); } catch { return false; }
        }
    }
}
