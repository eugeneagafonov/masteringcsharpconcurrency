using System;
using System.Collections.Concurrent;

namespace QueueServer
{
	public sealed class DataStore
	{
		private static readonly Lazy<ConcurrentDictionary<Guid, QueueTask>> lazy =
			 new Lazy<ConcurrentDictionary<Guid, QueueTask>>(() => new ConcurrentDictionary<Guid, QueueTask>());

		public static ConcurrentDictionary<Guid, QueueTask> Instance { get { return lazy.Value; } }

		private DataStore()
		{}
	}
}