# redis-com-client

This is a fork of redis-com-client by Diego Koga.
It exposes several extra Redis commands to Classic ASP, so not only string type keys can be stored, but also hashes. It also supports increment and decrement functions.



Redis Client for COM+ | StackExchange.Redis Wrapper.
This was made to be used on Classic ASP (ASP 3.0).

### Installation

Command to install the COM+ component: 

%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\regasm redis-com-client.dll /tlb:redis-com-client.tlb /codebase

On the ASP side you have not create the object. It can be done globally in the global.asa file like this:

- < OBJECT RUNAT=Server SCOPE=Application ID=Cache PROGID=CacheManager></OBJECT >

However, it also works if you want to create a scoped object:

- Set Cache = Server.CreateObject("CacheManager")

### Usage

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
