using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MZCachetastic
{
    public class Cachetastic
    {
        protected ConcurrentDictionary<string, CachePayload> Cache = new ConcurrentDictionary<string, CachePayload>();
		protected ConcurrentQueue<CachePayload> Bruce = new ConcurrentQueue<CachePayload>();

		protected double _lifetimeInMilliseconds;
		public double LifetimeInMilliseconds
		{
			get { return _lifetimeInMilliseconds; }
			set { Interlocked.Exchange(ref _lifetimeInMilliseconds, Math.Abs(value)); } 
		}

		public Cachetastic()
		{
			LifetimeInMilliseconds = TimeSpan.FromMinutes(5).TotalMilliseconds;
		}

        public int Count
        {
            get
            {
                return Cache.Count;
            }
        }

        public T Fetch<T>(string key, Func<T> fetchFunc)
        {
            return Fetch(key, String.Empty, fetchFunc);
        }

        public T Fetch<T>(string key, string hashcode, Func<T> fetchFunc)
        {
			// (Check every x seconds || size > y) && !Running
			PruneCache();

        	bool forceCacheUpdate = false;
			CachePayload cachePayload;
            bool cacheHit = Cache.TryGetValue(key, out cachePayload);
            if (cacheHit)
            {
                if (!hashcode.Equals(cachePayload.Hashcode))
                    forceCacheUpdate = true;
            }

            if (!cacheHit || forceCacheUpdate)
            {
                if (cacheHit)
                {
                    Cache.TryRemove(key, out cachePayload);
                }
            	cachePayload = new CachePayload { Key = key, Hashcode = hashcode, Value = fetchFunc() };
				//Cache.AddOrUpdate(key, cachePayload, (oldKey, oldValue) => cachePayload);
            	bool didAdd = Cache.TryAdd(key, cachePayload);
				if (didAdd)
				{
					Bruce.Enqueue(cachePayload);
				}
            }

            return (T)cachePayload.Value;
        }

        public void PruneCache()
        {
            

        	CachePayload cachePayload;

            //while (Bruce.TryPeek(out cachePayload))
            //{
            //    if (cachePayload.DateAdded.Subtract(DateTime.UtcNow).TotalMilliseconds > LifetimeInMilliseconds)
            //    {
            //        Bruce.TryDequeue(out cachePayload);
            //        Cache.TryRemove(cachePayload.Key, out cachePayload);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            for (Bruce.TryPeek(out cachePayload); cachePayload != null && DateTime.UtcNow.Subtract(cachePayload.DateAdded).TotalMilliseconds > LifetimeInMilliseconds; Bruce.TryPeek(out cachePayload))
            {
                Bruce.TryDequeue(out cachePayload);
                Cache.TryRemove(cachePayload.Key, out cachePayload);
            }
        }
    }
}
