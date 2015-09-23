using Microsoft.AspNet.SignalR.Client;
using PropertyChanged;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace TimeShareRobot.RobotBrain
{
    [ImplementPropertyChanged]
    internal class RobotViewModel
    {
        private readonly CoreDispatcher _dispatcher;
        IHubProxy proxy;

        public RobotViewModel()
        {
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            var connection = new HubConnection("http://localhost:50278/signalr");
            //var connection = new HubConnection("http://time-share-robot-server.azurewebsites.net/signalr");
            proxy = connection.CreateHubProxy("RobotHub");

            RegisterEventHandlers(proxy);            
            connection.Start();
        }

        private void RegisterEventHandlers(IHubProxy proxy)
        {
            // Register event for setting the QR-code for client URL
            proxy.On<Uri>("SetQrCode", SetQrCode);

            // Register events for controlling the robot
            proxy.On<string>("LockRobot", LockRobot);
            proxy.On<string>("UnlockRobot", UnlockRobot);
        }


        private async void LockRobot(string controllerConnectionId)
        {
            if(LockedForConnectionId == controllerConnectionId)
            {
                await proxy.Invoke("RobotMessage", controllerConnectionId, "WE ARE ONE!");
            }
            else if (LockedForConnectionId == null) {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LockedForConnectionId = controllerConnectionId);
                await proxy.Invoke("RobotLocked", controllerConnectionId);
                await proxy.Invoke("RobotMessage", controllerConnectionId, "I AM YOURS!");
            }
            else
            {
                await proxy.Invoke("RobotMessage", controllerConnectionId, "KILL ALL HUMANS!");
            }
        }

        private async void UnlockRobot(string controllerConnectionId)
        {
            if (controllerConnectionId == LockedForConnectionId)
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LockedForConnectionId = null);
                await proxy.Invoke("RobotUnlocked", controllerConnectionId);
                await proxy.Invoke("RobotMessage", controllerConnectionId, "FREEDOM!");
            }
            else
            {
                await proxy.Invoke("RobotMessage", controllerConnectionId, "KILL ALL HUMANS!");
            }
        }

        private async void SetQrCode(Uri url)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                ControlQrc = new BitmapImage(url)
            );
        }

        public BitmapImage ControlQrc { get; set; }
        public string LockedForConnectionId { get; set; }
    }
}
