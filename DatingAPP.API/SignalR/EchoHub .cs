using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace DatingAPP.API.SignalR
{
    public class EchoHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;
        public EchoHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;

        }
        public void Echo(string message)
        {
            //you're going to configure your client app to listen for this
            Clients.All.SendAsync("Send", message);

        }

        public Task SendMessage1(string user, string message)               // Two parameters accepted
        {
            return Clients.All.SendAsync("ReceiveOne", user, message);    // Note this 'ReceiveOne' 
        }
        public Task SendPrivateMessage(string user, string message)
        {
          
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }
    }
}