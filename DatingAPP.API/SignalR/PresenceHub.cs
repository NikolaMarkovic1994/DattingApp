using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingAPP.API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        // Hub implementiramo interfejs SignaLR
        // virtual metode se mogu overide
        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(Context.User.FindFirst(ClaimTypes.Name)?.Value, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.FindFirst(ClaimTypes.Name)?.Value);
            // Clients - klijenti koji su povezani na Hub
            // Othes oznacava odtale klijente
            // SendAsync da se posanje poruka
            // Da se ostalim klijentima posanje poruka da je korisnik poda nazivom ClaimTypes.Name online

            var currentUser = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUser);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            await _tracker.UserDisconnected(Context.User.FindFirst(ClaimTypes.Name)?.Value, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.FindFirst(ClaimTypes.Name)?.Value);
            // Clients - klijenti koji su povezani na Hub
            // Othes oznacava odtale klijente
            // SendAsync da se posanje poruka
            // Da se ostalim klijentima posanje poruka da je korisnik poda nazivom ClaimTypes.Name online
           var currentUser = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUser);

            await base.OnDisconnectedAsync(exception);

        }
    }
}