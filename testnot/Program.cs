using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR.Client;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace testnot
{
    class Program
    {

        static void Main(string[] args)
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
                SendNotification(message);
            });

            Console.Read();
            connection.DisposeAsync();
        }

        static void SendNotification(string message)
        {
            //ubuntu https://www.thegeekstuff.com/2010/12/ubuntu-notify-send/
            var xml = @"<toast>
                            <visual>
                                <binding template=""ToastImageAndText04"">
                                    <image id=""1"" src=""file:///C:\projects\testnot\images\image.png"" alt=""stock""/>
                                    <text id=""1"">Stock Info</text>
                                    <text id=""2"">Price evolution</text>
                                    <text id=""3"">https://dataexch.herokuapp.com/</text>

                                </binding>
                            </visual>
                        </toast>";
            var toastXml = new XmlDocument();
            toastXml.LoadXml(xml);
            var toast = new ToastNotification(toastXml);
            toast.Activated += (ToastNotification sender, object args) =>
            {
                OpenBrowser("https://dataexch.herokuapp.com/");
            };
            ToastNotificationManager.CreateToastNotifier("Stock info").Show(toast);

            // var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            // // var images = template.GetElementsByTagName("image");
            // // images[0].Attributes[1].InnerText  = "images/stock.jpg";

            // var textNodes = template.GetElementsByTagName("text");
            // textNodes.Item(0).InnerText = message;

            // var notifier = ToastNotificationManager.CreateToastNotifier("Stock infos");
            // var notification = new ToastNotification(template);
            // notification.Activated += (ToastNotification sender, object args) =>
            // {
            //     OpenBrowser("https://dataexch.herokuapp.com/");
            // };
            // notifier.Show(notification);
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
