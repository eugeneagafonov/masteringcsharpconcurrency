using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueServer
{
	public sealed class MessageQueue
	{
		private static readonly Lazy<ConcurrentQueue<QueueTask>> lazy =
				new Lazy<ConcurrentQueue<QueueTask>>(() => new ConcurrentQueue<QueueTask>());

		public static ConcurrentQueue<QueueTask> Instance { get { return lazy.Value; } }

		private MessageQueue()
		{
		}
	}
}
