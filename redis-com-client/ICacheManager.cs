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

        /*
        int decr(string key);
        int decrby(string key, int decrement);
        int incr(string key);
        int incrby(string key, int increment);

        void hdel(string key, string field);
        bool hexists(string key, string field);
        object hget(string key, string field);
        object hgetall(string key);
        int hlen(string key);
        void hset(string key, string field, object value);
        String[] hkeys(string key);
        Object[] hvals(string key);
        */
        void RemoveAll(string prefix);
        object this[string key] { get; set; }


        void SetExpiration(string key, int milliseconds);
    }
}