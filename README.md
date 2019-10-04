# redis-com-client

This is a fork of redis-com-client by Diego Koga, made to support more Redis commands from a COM+ client like Classic ASP.

Another improvement is that it's possible to provide a configuration for connecting to different Redis servers across a network, instead of only to localhost. Examples of these configurations can be found here:[https://stackexchange.github.io/StackExchange.Redis/Configuration.html#basic-configuration-strings](https://stackexchange.github.io/StackExchange.Redis/Configuration.html#basic-configuration-strings)

### Installation

The COM+ dll is 64 bit, so it requires the 64-bit regasm.exe to install.

`%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm redis-com-client.dll /tlb:redis-com-client.tlb /codebase`

Make sure the application pool for your ASP site has rights to the directory you are adding the dll in.

### Usage

In ASP the client can be used like this:

```visual-basic
Dim Redis
Set Redis = Server.CreateObject("CacheManager")
    ' Pass in a configuration to connect to a server
    Redis.Open("localhost")

    ' SetPermanent gives a key an indefinite TTL...
    call Redis.SetPermanent("key1", "abcde")

    Response.Write(Redis.Get("key1"))
    
    ' ...but you can always change the TTL later
    call Redis.Expire("key1", 60)    
    
    ' Or delete it alltogether
    Redis.Del("Key1")

    ' The key "stringkey" will have a TTL of 10 seconds
    call Redis.Set("stringkey", "abcde", 10)

    ' We can also put keys and values in hashes...
    call Redis.Hset("testhash", "firstfield", "firstvalue")
    call Redis.Hset("testhash", "secondfield", "secondvalue")
    
    ' ...and expire the entire hash

    call Redis.Expire("testhash", 60)

    dim result : result = Redis.Hget("testhash", "firstfield")
    Response.Write(result)

    ' Or delete a complete hash in one go   
    Redis.Del("testhash")

Set Redis = Nothing
```

### API

-**Add**

  Cache.Add "key1", "value"

 **or**

  Cache("key1") = "value"

-**Add with expiration**

  Cache.SetExpiration "key1", "value", 1000 'ms

-**Get**

  Cache.Get "key1"

**or**

  Cache("key1")

-**Remove**

  Cache.Remove "key1"

-**Remove All**

  Cache.RemoveAll()

### Common errors

### License












