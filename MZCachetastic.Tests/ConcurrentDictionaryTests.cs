using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MZCachetastic.Tests
{
    [TestClass]
    public class ConcurrentDictionaryTests
    {
        [TestMethod]
        public void CacheMiss_ShouldInvokeFunc_ShouldReturnExpectedResult()
        {
            const string key = "Linda";
            const string hashcode = "Honey";
            const string expected = "Sweet";

            string actual = new Cachetastic().Fetch(key, hashcode, () => InvokeCacheHit(expected) );

            Assert.AreEqual(expected, actual);
        }

		[TestMethod]
		public void CacheHit_ShouldNotInvokeFunc_ShouldReturnCachedResult()
		{
			const string key = "Linda";
			const string hashcode = "Honey";
			const string expected = "Sweet";

			var cachetastic = new Cachetastic();
			cachetastic.Fetch(key, hashcode, () => InvokeCacheHit(expected));
			string actual = cachetastic.Fetch(key, hashcode, () => InvokeCacheMiss(expected));

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CacheHit_WithDifferentHashcode_ShouldNotInvokeFunc_ShouldReturnCachedResult()
		{
			const string key = "Linda";
			const string hashcode = "Honey";
			const string expected = "Sweet";

			var cachetastic = new Cachetastic();
			cachetastic.Fetch(key, hashcode, () => InvokeCacheHit(expected));
			string actual = cachetastic.Fetch(key, hashcode + "x", () => InvokeCacheMiss(expected));

			Assert.AreNotEqual(expected, actual);
		}

		[TestMethod]
		public void NoHashCode_CacheMiss_ShouldInvokeFunc_ShouldReturnExpectedResult()
		{
			const string key = "Linda";
			const string hashcode = "Honey";
			const string expected = "Sweet";

			string actual = new Cachetastic().Fetch(key, hashcode, () => InvokeCacheHit(expected));

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void NoHashCode_CacheHit_ShouldNotInvokeFunc_ShouldReturnCachedResult()
		{
			const string key = "Linda";
			const string hashcode = "Honey";
			const string expected = "Sweet";

			var cachetastic = new Cachetastic();
			cachetastic.Fetch(key, hashcode, () => InvokeCacheHit(expected));
			string actual = cachetastic.Fetch(key, hashcode, () => InvokeCacheMiss(expected));

			Assert.AreEqual(expected, actual);
		}

        [TestMethod]
        public void CacheShouldBeEmpty_After_LifetimeExpires()
        {
            var cachetastic = new Cachetastic();
            cachetastic.LifetimeInMilliseconds = 500;
            for (int i = 0; i < 10000; i++)
            {
                int iTemp = i;
                cachetastic.Fetch(i.ToString(), () => iTemp);
            }

            System.Threading.Thread.Sleep(1000);

            cachetastic.Fetch("1", () => 1);

            // We actually expect a result of 1 here, as we need to add an item after the Lifetime for the Pruning to take place.
            Assert.AreEqual(1, cachetastic.Count);
        }

        protected string InvokeCacheHit(string expected) 
        {
            return expected;
        }

        protected string InvokeCacheMiss(string expectedPleaseModifyInternally) 
        {
            return expectedPleaseModifyInternally + DateTime.UtcNow;
        }
    }
}
