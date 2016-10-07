using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ContextsAndConfigureAwait
{
    class Program
    {
        private static Label _label;

        [STAThread]
        static void Main(string[] args)
        {
            CreateApplicationWindow();

            Console.ReadLine();
        }

        private static async void Click(object sender, EventArgs e)
        {
            _label.Content = "Starting asynchronous operation....";

            await SomeOperationAsync();
                //.ConfigureAwait(continueOnCapturedContext: false);

            _label.Content = "Asynchronous operation complete!";
        }

        static Task SomeOperationAsync()
        {
           return Task.Delay(TimeSpan.FromSeconds(5));
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
            button.Content = new TextBlock { Text = "Start asynchronous operation" };
            button.Click += Click;
            panel.Children.Add(_label);
            panel.Children.Add(button);
            win.Content = panel;
            app.Run(win);
        }
    }
}
