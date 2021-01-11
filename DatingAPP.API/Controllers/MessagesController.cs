using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Extensions;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.Mvc;


namespace DatingAPP.API.Controllers
{
    [ApiController]
    [Route("users/{userId}/[controller]")]
    public class MessagesController : ControllerBase
    
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet("/{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id){

              if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             {
                  return Unauthorized(); 
        
             }
             var messageFromRepo = await _repo.GetMessage(id);
             if (messageFromRepo == null)
             {
                 return NotFound();
             }
             return Ok(messageFromRepo);
        }


         [HttpGet]
        public async Task<IActionResult> GetMessageForUser(int userId, [FromQuery]MessageParams messageParams ){

              if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             {
                  return Unauthorized(); 
        
             }
             messageParams.UserId =userId;
             var messageFromRepo = await _repo.GetMessagesFromUser(messageParams);
            
             var message = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);

             Response.AddPagination(messageFromRepo.CurrentPage,messageFromRepo.PageSize
             , messageFromRepo.TotalCount,messageFromRepo.TotalPages);

             return Ok(message);

        }


          [HttpGet("tread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId ){

              if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             {
                  return Unauthorized(); 
        
             }
             
             var messageFromRepo = await _repo. GetMessageThread(userId,recipientId);
            
             var message = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);

           
             return Ok(message);

        }
        [HttpPost]
       public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repo.GetUser(userId);
                if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             {
                  return Unauthorized(); 
        
             }
             messageForCreationDto.SenderId = userId;

             var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);

             if(recipient==null){
                 return BadRequest("User not found");
             }
             var message = _mapper.Map<Message>(messageForCreationDto);

                _repo.Add(message);
            
            if( await _repo.SaveAll()){
                var messageToReturn =  _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage", new { id =message.Id}, messageToReturn); //OVDE JE GRESKA MORA SE PROVERITI
            
            }
            throw new Exception ("GReska pri cucanju poruke");
    }
     [HttpPost("{id}")]
       public async Task<IActionResult> DelateMessage(int Id, int userId)
        {
           
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             {
                  return Unauthorized(); 
        
             }
             
             var messageFromRepo = await _repo.GetMessage(Id);

             if(messageFromRepo.SenderId == userId){
                messageFromRepo.SenderDeleted = true;
             }
            if(messageFromRepo.RecipientId == userId){
                messageFromRepo.RecepientDeleted = true;
             }
            
            if(messageFromRepo.SenderDeleted && messageFromRepo.RecepientDeleted ){
                _repo.Delete(messageFromRepo);
             }
            
            if( await _repo.SaveAll()){
                
                return NoContent();
            
            }
            throw new Exception ("GReska pri brisanju poruke");
    }

      [HttpPost("{id}/read")]
       public async Task<IActionResult> MesageMarkAsRead(int userId, int Id)
        {
           
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             {
                  return Unauthorized(); 
        
             }
             
             var messageFromRepo = await _repo.GetMessage(Id);

             
            if(messageFromRepo.RecipientId != userId){
               return Unauthorized();
             }
            
            messageFromRepo.IsRead = true;
            messageFromRepo.DateRead= DateTime.Now;
            
             await _repo.SaveAll();
                
                return NoContent();
            
            
            
    }
}
}