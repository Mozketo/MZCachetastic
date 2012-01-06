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
