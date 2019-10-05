# redis-com-client

This is a fork of redis-com-client by Diego Koga, made to support more Redis commands from a COM+ client like Classic ASP.

Another improvement is that it's possible to provide a configuration for connecting to different Redis servers across a network, instead of only to localhost. Examples of these configurations can be found here:[https://stackexchange.github.io/StackExchange.Redis/Configuration.html#basic-configuration-strings](https://stackexchange.github.io/StackExchange.Redis/Configuration.html#basic-configuration-strings)

### Installation

Copy the dll and its dependencies to a directory somewhere the application pool your classic ASP site has rights. Then register the main redis-com-client.dll. The COM+ dll is 64 bit, so it requires the 64-bit regasm.exe to install.

`%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm redis-com-client.dll /tlb:redis-com-client.tlb /codebase`

Make sure the application pool for your ASP site has rights to the directory with the dll.

### Usage

In ASP the client can be used like this:

```visual-basic
Set Redis = Server.CreateObject("CacheManager")
    ' Pass in a configuration to connect to a server
    Redis.Open("localhost")

    ' SetPermanent gives a key an indefinite TTL...
    call Redis.SetPermanent("key1", "abcde")
    Response.Write("Key1 = " & Redis.Get("key1") & "<br/>")

    ' ...but you can always change the TTL later
    call Redis.Expire("key1", 60)
    Response.Write("Key1 TTL = " & Redis.TTL("key1") & "<br/>")

    ' Or delete it alltogether
    Redis.Del("key1")
    Response.Write("Key1 is now gone... " & Redis("key1") & "<br/>")

    ' The key "stringkey" will have a TTL of 10 seconds
    call Redis.Set("stringkey", "abcde", 10)

    ' We can also put keys and values in hashes...
    call Redis.Hset("testhash", "firstfield", "firstvalue")
    call Redis.Hset("testhash", "secondfield", "secondvalue")
    Response.Write("Testhash[firstfield] = " & Redis.Hget("testhash", "firstfield") & "<br/>")
    Response.Write("Testhash[secondfield] = " & Redis.Hget("testhash", "secondfield") & "<br/>")

    ' ...and expire the entire hash
    call Redis.Expire("testhash", 60)
    Response.Write("Testhash will expire in 60 seconds. The TTL is:" & Redis.TTL("testhash") & "<br/>")
    dim result : result = Redis.Hget("testhash", "firstfield")
    Response.Write("Here is our Testhash[firstfield] again:" & result & "<br/>")

    ' Or delete a complete hash in one go
    Redis.Del("testhash")
    Response.Write("Testhash is now gone...<br/>")
Set Redis = Nothing
```

### API

`Open(string configurationstring)`

Opens the connection to a Redis server using a configuration string. Configuration strings are described here: [https://stackexchange.github.io/StackExchange.Redis/Configuration.html#basic-configuration-strings](https://stackexchange.github.io/StackExchange.Redis/Configuration.html#basic-configuration-strings)

`Del(string key)`

Deletes a key from the server

`bool Exists(string key)`

Checks if a key by that name exists. Returns true or false.

`ExpireAt(string key, DateTime ExpireDatetime)`

Expires a key at a specific date and time

`Expire(string key, int seconds)`

Expires a key in the given amount of seconds

`int TTL(string key)`

Returns the TTL (time to live) of a key in seconds

`object Get(string key)`

Returns the value of a key

`Set(string key, object value, int SecondsToExpire)`

Sets the value of a key and the TTL in seconds

`SetPermanent(string key, object value)`

Sets the value of a key. The TTL will be set to -1, which means the key will be available permanently.

`Persist(string key)`

Sets a key to be permanently available

`string Type(string key)`

Returns the type of a key. For example 'string'.

`double Decr(string key)`

Decrements the value of the given key by 1. This key should hold an integer or a double value. Returns the resulting value.

`double DecrBy(string key, double decrement)`

Decrements the value of the given key by a given number. This key should hold an integer or a double value.Returns the resulting value.

`double Incr(string key)`

Increments the value of the given key by 1. This key should hold an integer or a double value.Returns the resulting value.

`double IncrBy(string key, double increment)`

Increments the value of the given key by a given number. This key should hold an integer or a double value.Returns the resulting value.

`Hdel(string key, string field)`

Deletes a specified field in a hash

`bool Hexists(string key, string field)`

Checks if a specified field in a hash exists. Returns true or false.

`object Hget(string key, string field)`

Returns the value of a field in a hash

`long Hlen(string key)`

Returns the number of fields in a hash

`bool Hset(string key, string field, string value)`

Sets the value of a field in a hash.

`RemoveKeysWithPrefix(string prefix)`

If you prefix the name of a set of keys with the same string, for example "CACHE:", you can easily delete all of these keys at once using this command. It wil look for all keys with the given prefix and delete them all.

`object this(string key) `

This is a shortcut for `SetPermanent(string key, object value)` and `Get(string key)` . For example:

Instead of writing: `Redis.SetPermanent("mykey", "myvalue")`

you can write: `Redis("mykey") = "myvalue"`

In the same way `Redis.Get("mykey")`

can be written as `Redis("mykey")`

`SetExpiration(string key, int milliseconds)`

This will allow you to set the expiration of a key in milliseconds instead of seconds

### Common errors

```
Server object error 'ASP 0177 : 800401f3' 
Server.CreateObject Failed 
/redistest.asp, line 7 
800401f3 
```

This message occurs when the component isn't registered. Register it using regasm like this:

`%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm [PATH_TO_THE_DLL] /tlb:redis-com-client.tlb /codebase`

```
Server object error 'ASP 0177 : 80070002' 
Server.CreateObject Failed 
/redistest.asp, line 7 
80070002 
```

You probably registered the component, but forgot the `/tlb` and `/codebase` part

```
StackExchange.Redis.StrongName error '80131500' 
It was not possible to connect to the redis server(s); to create a disconnected multiplexer, disable AbortOnConnectFail. SocketFailure on PING 
/redistest.asp, line 8
```

You should have a Redis server running on the specified computer. You can run Redis using Windows Subsystem for Linux, by running it on a Linux computer in your network or by finding the Windows Native Redis server online and running it on a Windows computer. 

### License

Licensed under the EUPL - European Union Public License 1.2.

**`EUPL-1.2`** Copyright (c) 2019 Erik Oosterwaal


This wrapper uses the StackExchange.Redis general purpose Redis client, which is licensed under the MIT License (MIT). More information can be found here: [https://github.com/StackExchange/StackExchange.Redis/blob/master/LICENSE](https://github.com/StackExchange/StackExchange.Redis/blob/master/LICENSE)






