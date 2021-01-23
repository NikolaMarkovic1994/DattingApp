using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Model;
using DatingAPP.API.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DatingAPP.API.Controllers
{
    [ApiController]
    [Route("message-hub")]
    public class MessageHubControler : ControllerBase
    {
        private readonly IHubContext<EchoHub> _hubContext;
        
        private readonly IMapper _mapper;
        private readonly IDatingRepository _repo;

        public MessageHubControler(IHubContext<EchoHub> hubContext, IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
           
            _hubContext = hubContext;
        }

        //path looks like this: https://localhost:44379/api/chat/send
        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] MessageHubDto msg)
        {

          

           

            await _hubContext.Clients.All.SendAsync("ReceiveOne", msg.user, msg.Content);
            return Ok();
        }

       [HttpGet("{id}/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int Id, int recipientId ){

             
             
             var messageFromRepo = await _repo. GetMessageThread(Id,recipientId);
            
             var message = _mapper.Map<IEnumerable<MessageHubDto>>(messageFromRepo);

           
             return Ok(message);

        }
    }
}