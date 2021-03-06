﻿using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MZCachetastic.Tests
{
	[TestClass]
	public class SpeedTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			var cachetastic = new Cachetastic
			                  	{
			                  		Lifetime = TimeSpan.FromSeconds(1)
			                  	};
			var stopwatch = Stopwatch.StartNew();

			// Load the Cache with Garbage
			for (int i = 0; i < 1000000; i++)
			{
				int iTemp = i;
				cachetastic.Get(i.ToString(), () => iTemp);
			}

			Debug.WriteLine(String.Format("Time to Load (ms): {0}", stopwatch.ElapsedMilliseconds));
		}

		[TestMethod]
		public void TestMethod2()
		{
			var cachetastic = new Cachetastic();
			var stopwatch = Stopwatch.StartNew();

			// Load the Cache with Garbage
			for (int i = 0; i < 1000000; i++)
			{
				int iTemp = i;
				cachetastic.Get(i.ToString(), () => iTemp);
			}

			for (int i = 0; i < 1000000; i++)
			{
				int iTemp = i;
				cachetastic.Get(i.ToString(), () => iTemp);
			}

			Debug.WriteLine(String.Format("Time to Load (ms): {0}", stopwatch.ElapsedMilliseconds));
		}

		[TestMethod]
		public void TestMethod3()
		{
			var cachetastic = new Cachetastic
			{
				Lifetime = TimeSpan.FromSeconds(1)
			};
			var stopwatch = Stopwatch.StartNew();

			// Load the Cache with Garbage
			for (int i = 0; i < 1000000; i++)
			{
				int iTemp = i;
				cachetastic.Get(i.ToString(), () => iTemp);
			}

			Thread.Sleep(1000);

			for (int i = 0; i < 1000000; i++)
			{
				int iTemp = i;
				cachetastic.Get(i.ToString(), () => iTemp);
			}

			Debug.WriteLine(String.Format("Time to Load (ms): {0}", stopwatch.ElapsedMilliseconds));
		}

        [TestMethod]
        public void TestMethod4()
        {
            var cachetastic = new Cachetastic();
            var stopwatch = Stopwatch.StartNew();

            // Load the Cache with Garbage
            for (int i = 0; i < 1000000; i++)
            {
                int iTemp = i;
				cachetastic.Get(i.ToString(), () => iTemp);
            }

            for (int i = 0; i < 1000000; i++)
            {
                int iTemp = i;
				cachetastic.Get(i.ToString(), "x", () => iTemp);
            }

            var elapsed = stopwatch.ElapsedMilliseconds;
            Trace.WriteLine(String.Format("Time to Load (ms): {0}", elapsed));
        }
	}
}
