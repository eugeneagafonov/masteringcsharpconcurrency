using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace AsyncCodeToTest
{
	public static class TaskExtensions
	{
		public static async Task TimeoutAfter(this Task task, int millisecondsTimeout)
		{
			if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
			{
				await task;
			}
			else
			{
				throw new TimeoutException();
			}
		}

		public static Task<T> TimeoutAfter<T>(this Task<T> task, int millisecondsTimeout)
		{
			return task.ToObservable().Timeout(TimeSpan.FromMilliseconds(millisecondsTimeout)).ToTask();
		}
	}
}