using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using redis_com_client;

namespace redis_com_client_test
{
    [TestClass]
    public class BasicTest
    {

        private RedisClient _manager;

        [TestInitialize]
        public void Initialize()
        {
            _manager = new RedisClient();
            _manager.Open("localhost");
        }


        [TestMethod]
        public void test_SetPermanent_and_Get()
        {
            _manager.SetPermanent("key1", "abcde");
            Assert.AreEqual("abcde", _manager.Get("key1"));
        }

        [TestMethod]
        public void test_Del()
        {
            _manager.SetPermanent("key1", "abcde");
            _manager.Del("key1");
            Assert.IsNull(_manager.Get("key1"));
        }

        [TestMethod]
        public void CheckTTL()
        {
            _manager.SetPermanent("dk", "123");
            _manager.Expire("dk", 10);
            int timetolive = _manager.TTL("dk");
            Assert.IsTrue(timetolive >= 5 && timetolive <= 10);
        }

        [TestMethod]
        public void test_Set_and_Persist()
        {
            _manager.Set("persistentkey", "abcde", 90);
            _manager.Persist("persistentkey");
            int timetolive = _manager.TTL("persistentkey");
            Assert.IsTrue(timetolive == -1);
        }

        [TestMethod]
        public void test_string_type()
        {
            _manager.Set("stringkey", "abcde", 10);
            string valuetype = _manager.Type("stringkey");
            Assert.IsTrue(valuetype.ToLower() == "string");
        }

        [TestMethod]
        public void test_incr()
        {
            _manager.Set("MyNumber", 3, 10);
            double newValue = _manager.Incr("MyNumber");
            Assert.IsTrue(newValue == 4);
            Assert.AreEqual("4", _manager.Get("MyNumber"));
        }

        [TestMethod]
        public void test_Hset_Hget()
        {
            _manager.Hset("testhash", "firstfield", "firstvalue");
            _manager.Expire("testhash", 60);
            object result = (string)_manager.Hget("testhash", "firstfield");
            Assert.AreEqual("firstvalue", result);
        }

        [TestMethod]
        public void GetByObject()
        {
            _manager.SetPermanent("dk", "123");
            Assert.AreEqual("123", _manager["dk"]);
        }

        [TestMethod]
        public void SetByObject()
        {
            _manager["dk"] = "123";
            Assert.AreEqual("123", _manager["dk"]);
        }

        [TestMethod]
        public void Array()
        {
            var array = new object[,] {{1, 2, 3}, { "a", "b", "c" }, { "aa", "bb", "cc" } };

            _manager["arraytest"] = array;

            var x = (object[,])_manager["arraytest"];
            Assert.AreEqual(int.Parse("1"), x[0, 0]); //It guarantees int32 to VBScropt compability.
        }

        [TestMethod]
        public void ArraySpecialChar()
        {
            var specialChar = "#$%ˆ@";
            var array = new object[,] { { 1, 2, 3 }, { specialChar, "b", "c" } };

            _manager["arraytest"] = array;

            var x = (object[,])_manager["arraytest"];
            Assert.AreEqual(specialChar, x[1, 0]);
        }

        [TestMethod]
        public void ArrayHtml()
        {
            var html = "<script>alert();</script>";
            var array = new object[,] { { 1, 2, 3 }, { html, "b", "c" } };

            _manager["arraytest"] = array;

            var x = (object[,])_manager["arraytest"];
            Assert.AreEqual(html, x[1, 0]);
        }

        [TestMethod]
        public void RemoveAllFromThisKey()
        {
            var manager2 = new RedisClient();
            manager2.Open("localhost");
            manager2.SetPermanent("myPrefix2:firstname", "22222");
            manager2.SetPermanent("myPrefix2:lastname", "33333");

            _manager.SetPermanent("myPrefix:firstname", "firstname123");
            _manager.SetPermanent("myPrefix:lastname", "lastname123");
            _manager.RemoveKeysWithPrefix("myPrefix:");

            Assert.IsNull(_manager["myPrefix:firstname"]);
            Assert.IsNull(_manager["myPrefix:lastname"]);
            Assert.IsNotNull(manager2["myPrefix2:firstname"]);
            Assert.IsNotNull(manager2["myPrefix2:lastname"]);
        }

        [TestMethod]
        public void Exists()
        {
            _manager.SetPermanent("exists", "12344");
            Assert.IsTrue(_manager.Exists("exists"));
        }

        [TestMethod]
        public void NotExists()
        {
            Assert.IsFalse(_manager.Exists("notexists"));
        }

        [TestMethod]
        public void Remove()
        {
            _manager.SetPermanent("onekey", "12344");
            _manager.Del("onekey");
            Assert.IsFalse(_manager.Exists("onekey"));
        }

        [TestMethod]
        public void SetExpirationExistingKey()
        {
            _manager.SetPermanent("key4", "12344");
            Assert.IsTrue(_manager.Exists("key4"));
            _manager.SetExpiration("key4",1000);
            Thread.Sleep(2000);
            Assert.IsNull(_manager["key4"]);
        }

        [TestMethod]
        public void AddNullValue()
        {
            _manager.SetPermanent("null", null);
            Assert.IsTrue(_manager.Exists("null"));
        }

        [TestMethod]
        public void ReplaceDbNull()
        {
            var array = new object[,] {{10, 20}, {"asdf", DBNull.Value}};
            _manager["DBNull"] = array;

            var result = (object[,])_manager["DBNull"];
            Assert.IsNotNull(result);
            Assert.IsNull(result[1,1]);
        }

        [TestMethod]
        public void AddSameKeyTwice()
        {
            _manager["twice"] = 1;
            Thread.Sleep(500);
            _manager["twice"] = "asdf";
            Assert.AreEqual("asdf", _manager["twice"]);
        }

        [TestMethod]
        public void Concurrent()
        {
            ArrayHtml();
            var sb = new StringBuilder();
            Parallel.For((long) 0, 1000, i =>
            {
                sb.AppendFormat("i: {0}{1}", i, Environment.NewLine);
                var sw = new Stopwatch();
                sw.Start();
                object x = _manager["arraytest"];
                sw.Stop();
                sb.AppendFormat("i: {0} - time: {1}ms", i, sw.ElapsedMilliseconds);
            });
            Console.WriteLine(sb);
        }

        [TestMethod]
        public void MyTableTwoDim()
        {
            var array = new object[,] { { 1, 2, 3 }, { "a", "b", "c" } };
            var table = new MyTable(array);

            var newArray  = (object[,])table.GetArray();
            Assert.AreEqual(array[0, 0], newArray[0, 0]);
            Assert.AreEqual(array[1, 1], newArray[1, 1]);
        }

        [TestMethod]
        public void MyTableOneDim()
        {
            var array = new object[] { 1, 2, 3 } ;
            var table = new MyTable(array);

            var newArray = (object[])table.GetArray();
            Assert.AreEqual(array[0], newArray[0]);
            Assert.AreEqual(array[1], newArray[1]);
            Assert.AreEqual(array[2], newArray[2]);
        }
    }
}
