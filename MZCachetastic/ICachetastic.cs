using System;

namespace MZCachetastic
{
    public interface ICachetastic
    {
        /// <summary>
        /// Define how long an item should be held in the cache.
        /// </summary>
        TimeSpan Lifetime { get; set; }

        /// <summary>
        /// Gets the count of the number of items currently being cached.
        /// </summary>
        int Count { get; }

        T Get<T>(string key, Func<T> fetchCallback);
        T Get<T>(string key, string hashcode, Func<T> fetchCallback);

        /// <summary>
        /// Invalidates the cached item based on the key. This will remove the item from the cache.
        /// </summary>
        /// <param name="key">The key of the cached item to invalidate.</param>
        /// <returns>Returns true if the cached item has been removed from the cache. False if the item for the key was not removed (Note: False may mean the item wasn't cached).</returns>
        bool Remove(string key);
    }
}