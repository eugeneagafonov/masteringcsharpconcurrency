using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace UIPerformance
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
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
            button.Content = "Start asynchronous operations";
            button.Click += Click;
            panel.Children.Add(_label);
            panel.Children.Add(button);
            win.Content = panel;
            app.Run(win);

            Console.ReadLine();
        }

        async static void Click(object sender, EventArgs e)
        {
            _label.Content = "Calculating...";

            var dispatcher = Dispatcher.CurrentDispatcher;

            //If you uncomment this, the performance will be equal
            //await Task.Delay(1).ConfigureAwait(false);

            TimeSpan resultWithContext = await Test();
            //If you uncomment this there will be no difference 
            //TimeSpan resultWithContext = await Test().ConfigureAwait(false);
            TimeSpan resultNoContext = await TestNoContext();
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("With the context: {0}", resultWithContext));
            sb.AppendLine(string.Format("Without the context: {0}", resultNoContext));
            sb.AppendLine(string.Format("Ratio: {0:0.00}",
                resultWithContext.TotalMilliseconds / resultNoContext.TotalMilliseconds));

            dispatcher.Invoke( () =>
            {
                _label.Content = sb.ToString();
            });
        }

        async static Task<TimeSpan> Test()
        {
            const int iterationsNumber = 100000;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterationsNumber; i++)
            {
                var t = Task.Run(() => { });
                await t;
            }
            sw.Stop();
            return sw.Elapsed;
        }

        async static Task<TimeSpan> TestNoContext()
        {
            const int iterationsNumber = 100000;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterationsNumber; i++)
            {
                var t = Task.Run(() => { });
                await t.ConfigureAwait(
                    continueOnCapturedContext: false);
            }
            sw.Stop();
            return sw.Elapsed;
        }

        private static Label _label;
    }
}
