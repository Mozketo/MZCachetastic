using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MZCachetastic
{
	public class Cachetastic : ICachetastic
	{
		private double _lifetimeInMilliseconds;

		protected ConcurrentDictionary<string, CachePayload> Cache = new ConcurrentDictionary<string, CachePayload>();
		protected ConcurrentQueue<CachePayload> CacheLifetime = new ConcurrentQueue<CachePayload>();

		public Cachetastic()
		{
			Lifetime = TimeSpan.FromMinutes(5);
		}

		public Cachetastic(TimeSpan lifetime)
		{
			Lifetime = lifetime;
		}

		/// <summary>
		/// Define how long an item should be held in the cache.
		/// </summary>
		public TimeSpan Lifetime
		{
			get { return TimeSpan.FromMilliseconds(_lifetimeInMilliseconds); }
			set { Interlocked.Exchange(ref _lifetimeInMilliseconds, Math.Abs(value.TotalMilliseconds)); }
		}

		/// <summary>
		/// Gets the count of the number of items currently being cached.
		/// </summary>
		public int Count
		{
			get { return Cache.Count; }
		}

        /// <summary>
        /// Attempt to get the cached item based on a key. If the key is not found in the cache then the
        /// fetchCallback Func will be executed.
        /// </summary>
        /// <returns>The result of the cache lookup or failing that the fetchCallback.</returns>
        public T Get<T>(string key, Func<T> fetchCallback)
		{
            return Get(key, String.Empty, fetchCallback);
		}

        /// <summary>
        /// Attempt to get the cached item based on a key. If the key is not found in the cache then the
        /// fetchCallback Func will be executed. If the hashcode differs between the 
        /// </summary>
        /// <returns>The result of the cache lookup or failing that the fetchCallback.</returns>
        public T Get<T>(string key, string hashcode, Func<T> fetchCallback)
		{
			PruneAgedItemsFromCache();

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
				// Testing over 10,000,000 cached items a Cache.TryRemove()/TryAdd() was more efficient by 1s (on an i7 2.8GHz) than using AddOrUpdate().
				if (cacheHit)
				{
					Cache.TryRemove(key, out cachePayload);
				}
				cachePayload = new CachePayload { Key = key, Hashcode = hashcode, Value = fetchCallback() };
				bool didAdd = Cache.TryAdd(key, cachePayload);
				if (didAdd)
				{
					CacheLifetime.Enqueue(cachePayload);
				}
			}

			return (T) cachePayload.Value;
		}

		/// <summary>
		/// Invalidates the cached item based on the key. This will remove the item from the cache.
		/// </summary>
		/// <param name="key">The key of the cached item to invalidate.</param>
		/// <returns>Returns true if the cached item has been removed from the cache. False if the item for the key was not removed (Note: False may mean the item wasn't cached).</returns>
		public bool Remove(string key)
		{
			CachePayload cachePayload;
			return Cache.TryRemove(key, out cachePayload);
		}

		/// <summary>
		/// Check to see the items in the Cache that are older than the Lifetime. Cached items older than the Lifetime will be removed from the cache.
		/// </summary>
		protected void PruneAgedItemsFromCache()
		{
			CachePayload cachePayload;
            for (CacheLifetime.TryPeek(out cachePayload); cachePayload != null && DateTime.UtcNow.Subtract(cachePayload.DateAdded) > Lifetime; CacheLifetime.TryPeek(out cachePayload))
            {
                CachePayload dequeueCachePayload;
                CacheLifetime.TryDequeue(out dequeueCachePayload);
                Cache.TryRemove(cachePayload.Key, out cachePayload);
            }
		}
	}
}