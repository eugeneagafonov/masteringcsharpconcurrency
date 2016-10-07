using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AsyncInUI
{
    public static class TaskExtensions
    {
        public static T WaitWithNestedMessageLoop<T>(this Task<T> task)
        {
            var nested = new DispatcherFrame();
            task.ContinueWith(_ => nested.Continue = false, TaskScheduler.Default);
            Dispatcher.PushFrame(nested);
            return task.Result;
        }
    }
}
