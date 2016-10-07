using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurrencyBook.Samples
{
public class SynchronousTaskScheduler : TaskScheduler
{
	protected override IEnumerable<Task> GetScheduledTasks()
	{
		return Enumerable.Empty<Task>();
	}

	protected override void QueueTask(Task task)
	{
		TryExecuteTask(task);
	}

	protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return TryExecuteTask(task);
	}

	public override int MaximumConcurrencyLevel
	{
		get { return 1; }
	}
}
}