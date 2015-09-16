using System;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using TimeShareRobot.Server.UrlShortenerApi;

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
            Clients.Client(robotConnectionId).lockRobot(Context.ConnectionId);
        }

        public void UnlockRobot(string robotConnectionId)
        {
            Clients.Client(robotConnectionId).unlockRobot(Context.ConnectionId);
        }

        // Events from robot
        public void RobotLocked(string lockedForConnectionId)
        {
            Clients.Client(lockedForConnectionId).robotLocked();
        }

        public void RobotUnlocked(string lockedForConnectionId)
        {
            Clients.Client(lockedForConnectionId).robotUnlocked();
        }

        public void RobotMessage(string robotConnectionId, string message)
        {
            Clients.Client(robotConnectionId).robotMessage(message);
        }


        // Set up on connection
        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var qrcUrl = _urlShortener.GetQrCodeForUrl("http://localhost:53956/" + connectionId);

            Clients.Client(connectionId).setQrCode(new Uri(qrcUrl, UriKind.Absolute));
            return base.OnConnected();
        }
    }
}