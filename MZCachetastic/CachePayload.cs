using System;

namespace MZCachetastic
{
	public class CachePayload
	{
		public object Value { get; set; }
		public string Key { get; set; }
		public string Hashcode { get; set; }
		public DateTime DateAdded { get; protected set; }

		public CachePayload()
		{
			DateAdded = DateTime.UtcNow;
		}
	}
}
