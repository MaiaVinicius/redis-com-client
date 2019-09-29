using System;
using System.Runtime.InteropServices;

namespace redis_com_client
{
    [ComVisible(true)]
    [Guid("c8109c73-2528-4e90-a999-81abd1fc7a70")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ICacheManager
    {
        void Del(string key);
        bool Exists(string key);
        void ExpireAt(string key, DateTime ExpireDatetime);
        void Expire(string key, int seconds);
        int TTL(string key);

        object Get(string key);
        void Set(string key, object value, int SecondsToExpire);
        void SetPermanent(string key, object value);
        void Persist(string key);
        string Type(string key);
        
        double Decr(string key);
        double DecrBy(string key, double decrement);
        double Incr(string key);
        double IncrBy(string key, double increment);
        
        void Hdel(string key, string field);
        bool Hexists(string key, string field);
        object Hget(string key, string field);
        object Hgetall(string key);
        long Hlen(string key);
        bool Hset(string key, string field, string value);
        
        /*
        String[] Hkeys(string key);
        Object[] Hvals(string key);
        */

        void RemoveKeysWithPrefix(string prefix);
        object this[string key] { get; set; }


        void SetExpiration(string key, int milliseconds);
    }
}