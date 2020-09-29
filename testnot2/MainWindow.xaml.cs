using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace testnot2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            this.Message = "No updates...";
            this.DataContext = this;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);


            Task.Run(() =>
            {
                //Set connection
                var connection = new HubConnectionBuilder()
                //.WithUrl("https://localhost:5001/messages")
                .WithUrl("https://dataexch.herokuapp.com/messages")
                .Build();

                connection.StartAsync()
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                    }
                    else
                    {
                        Console.WriteLine("Connected");
                    }

                }).Wait();

                connection.On("ReceiveMessage", (string message) =>
                {
                    Console.WriteLine(message);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.Message = message;
                        this.WindowState = WindowState.Normal;
                        this.Activate();
                    });
                });
            });
        }

        public String Message
        {
            get { return (String)this.GetValue(MessageProperty); }
            set { this.SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
          "Message", typeof(String), typeof(MainWindow), new PropertyMetadata(""));

        void OpenSite(Object sender, RoutedEventArgs args)
        {
            OpenBrowser("https://dataexch.herokuapp.com/");
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
