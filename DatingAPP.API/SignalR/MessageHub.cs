using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.SignalR;

namespace DatingAPP.API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IDatingRepository _messageReop;
        private readonly IMapper _mapper;
        public MessageHub(IDatingRepository messageReop, IMapper mapper)
        {
            _mapper = mapper;
            _messageReop = messageReop;

        }

        public override async Task OnConnectedAsync(){
            
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
           // string myValue = otherUser.Split('=')[1];
            var test = Context.User.FindFirst(ClaimTypes.Name)?.Value;
            var otherUserName = await _messageReop.GetUser(int.Parse(otherUser),false);


             var groupName = GetGroupName(test,otherUserName.UserName);
            //var groupName = GetGroupName(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,otherUserName.Id.ToString());
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
           var group = await AddToGroup(groupName);
            // var testId2=int.Parse(myValue);
            // var testID = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var messages = await _messageReop.GetMessageThread(int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value),int.Parse(otherUser));
             
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

            

            
            
        }
        /*
        *************
        */
         public async Task SendMessage(int id,MessageForCreationDto createMessageDto)
        {
            var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
            var usernameId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            if (usernameId == createMessageDto.RecipientId)
                throw new HubException("You cannot send messages to yourself");

            var sender = await _messageReop.GetUser(usernameId,false);
            var recipient = await _messageReop.GetUser(createMessageDto.RecipientId,false);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                
                RecipientId = recipient.Id,
                SenderId = sender.Id,
                
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _messageReop.GetMessageGroup(groupName);

            if (group.Connection.Any(x => x.UserName == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
               
            }

            _messageReop.Add(message);

            if (await _messageReop.SaveAll())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageToReturnDto>(message));
            }
        }

         public override async Task OnDisconnectedAsync(Exception exception)
        {

          await base.OnDisconnectedAsync(exception);

        }
        /*
        ********************************
        ********************************
        */
        private string GetGroupName(string caller, string other){

                var stringCompare = string.CompareOrdinal(caller,other)<0;
                return stringCompare ? $"{caller}-{other}": $"{other}-{caller}";
        }
        /*
        *
        *
        *
        */
 private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _messageReop.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.FindFirst(ClaimTypes.Name)?.Value);

            if (group == null)
            {
                group = new Group(groupName);
                _messageReop.AddGroup(group);
            }

            group.Connection.Add(connection);

            if (await _messageReop.SaveAll()) return group;

            throw new HubException("Failed to join group");
        }
        /*
        *
        *
        *
        *
        */
        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageReop.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connection.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _messageReop.RemoveConnection(connection);
            if (await _messageReop.SaveAll()){ return group;}

            throw new HubException("Failed to remove from group");
        }

       
    }
}