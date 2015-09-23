using System;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using TimeShareRobot.Server.UrlShortenerApi;
using TimeShareRobot.Server.Properties;
using System.Diagnostics;

namespace TimeShareRobot.Server
{
    public class RobotHub : Hub
    {
        private UrlShortener _urlShortener;

        public RobotHub()
        {
            _urlShortener = new UrlShortener();
        }

        // Commands from remote controll
        public void LockRobot(string robotConnectionId)
        {
            Clients.Others.lockRobot(Context.ConnectionId);
        }

        public void UnlockRobot(string robotConnectionId)
        {
            Clients.Client(robotConnectionId).unlockRobot(Context.ConnectionId);
        }

        // Events from robot
        public void RobotLocked(string controllerConnectionId)
        {
            Clients.Client(controllerConnectionId).robotLocked(Context.ConnectionId);
        }

        public void RobotUnlocked(string controllerConnectionId)
        {
            Clients.Client(controllerConnectionId).robotUnlocked(Context.ConnectionId);
        }

        public void RobotMessage(string controllerConnectionId, string message)
        {
            Clients.Client(controllerConnectionId).robotMessage(Context.ConnectionId, message);
        }


        // Set up on connection
        public override Task OnConnected()
        {
            Debug.WriteLine("Connected: " + Context.ConnectionId);
            var connectionId = Context.ConnectionId;
            var qrcUrl = _urlShortener.GetQrCodeForUrl(string.Format(Settings.Default.RemoteControlAppUrlFormat, connectionId));

            Clients.Client(connectionId).setQrCode(new Uri(qrcUrl, UriKind.Absolute));
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Debug.WriteLine("Disconnected: " + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Debug.WriteLine("Reconnected: " + Context.ConnectionId);
            return (base.OnReconnected());
        }
    }
}