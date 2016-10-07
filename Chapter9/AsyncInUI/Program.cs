using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AsyncInUI
{
    internal class Program
    {
        private static Label _label;

        [STAThread]
        private static void Main(string[] args)
        {
            CreateApplicationWindow();

            Console.ReadLine();
        }

        private static void SyncClick(object sender, EventArgs e)
        {
            _label.Content = string.Empty;
            try
            {
                #region Synchronization context

                //string result = TaskMethod(TaskScheduler.FromCurrentSynchronizationContext()).Result;

                #endregion

                #region Nested Message Loop

                string result = TaskMethod(TaskScheduler.FromCurrentSynchronizationContext()).WaitWithNestedMessageLoop();

                #endregion

                #region GetAwaiter

                //string result = TaskMethod().GetAwaiter().GetResult();

                #endregion

                //string result = TaskMethod().Result;
                _label.Content = result;
            }
            catch (Exception ex)
            {
                _label.Content = ex.Message;
            }
        }

        private static void AsyncClick(object sender, EventArgs e)
        {
            _label.Content = string.Empty;
            Mouse.OverrideCursor = Cursors.Wait;
            Task<string> task = TaskMethod();

            task.ContinueWith(t =>
                {
                    _label.Content = t.Exception.InnerException.Message;
                    Mouse.OverrideCursor = null;
                },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }

        private static void AsyncOkClick(object sender, EventArgs e)
        {
            _label.Content = string.Empty;
            Mouse.OverrideCursor = Cursors.Wait;
            Task<string> task = TaskMethod(
                TaskScheduler.FromCurrentSynchronizationContext());

            task.ContinueWith(t => Mouse.OverrideCursor = null,
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static Task<string> TaskMethod()
        {
            return TaskMethod(TaskScheduler.Default);
        }

        private static Task<string> TaskMethod(TaskScheduler scheduler)
        {
            Task delay = Task.Delay(TimeSpan.FromSeconds(5));

            return delay.ContinueWith(t =>
            {
                string str = string.Format("Task is running on a thread id {0}. Is thread pool thread: {1}",
                    Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
                _label.Content = str;
                return str;
            }, scheduler);
        }

        private static void CreateApplicationWindow()
        {
            var app = new Application();
            var win = new Window();
            var panel = new StackPanel();
            var button = new Button();
            _label = new Label();
            _label.FontSize = 32;
            _label.Height = 200;
            button.Height = 100;
            button.FontSize = 32;
            button.Content = new TextBlock { Text = "Start synchronous operation" };
            button.Click += SyncClick;
            panel.Children.Add(_label);
            panel.Children.Add(button);
            button = new Button();
            button.Height = 100;
            button.FontSize = 32;
            button.Content = new TextBlock { Text = "Start asynchronous operation" };
            button.Click += AsyncClick;
            panel.Children.Add(button);
            button = new Button();
            button.Height = 100;
            button.FontSize = 32;
            button.Content = new TextBlock { Text = "Start good asynchronous operation" };
            button.Click += AsyncOkClick;
            panel.Children.Add(button);
            win.Content = panel;
            app.Run(win);
        }
    }
}
