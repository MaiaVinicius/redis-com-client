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

        void Add(string key, object value);
        object Get(string key);
        void RemoveAll(string prefix);
        object this[string key] { get; set; }


        void SetExpiration(string key, int milliseconds);
    }
}