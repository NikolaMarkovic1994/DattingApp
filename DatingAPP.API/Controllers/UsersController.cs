using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Extensions;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPP.API.Controllers
{
    //[ServiceFilter(typeof(LogUserActiv))]
   // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        
       // [Authorize]
        [HttpGet("userss")]
        public async Task<IActionResult> GetUsers([FromQuery ]UserParams userParams)
        {
            var curentUserId= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromReop = await _repo.GetUser(curentUserId,true);
            userParams.UserId = curentUserId;
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender =userFromReop.Gender == "male" ? "female" : "male";
            }
            var users = await _repo.GetUsers(userParams);
            var usersToReturn= _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
            return Ok(usersToReturn);


        }
        [HttpGet("{id}" ,  Name = "GetUser")]
        public async Task<IActionResult> GetUsers1(int id)
        {
            var isCurrentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)==id;
            // var users = await _repo.GetUsers(); 
            // return Ok(users);
            var user = await _repo.GetUser(id,isCurrentUser);
            var usersToReturn= _mapper.Map<UserForDetaileDto>(user);

            return Ok(usersToReturn);

        }
        [HttpPut("{id}/{userName}")]
        public async Task<IActionResult> UpadateUser(int id,string userName, UserForUpadateDto userForUpadateDto)
         {
        //     if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //     {
        //         return Unauthorized(); MORAMO POGELDATI 
        
        //    }
            

            var userFromRepo = await _repo.GetUser(id,true);
            var userNameFromRepo = await _repo.GetUserName(userName);
            
            if(userFromRepo.Id != userNameFromRepo.Id)
            {return Unauthorized();}

            _mapper.Map(userForUpadateDto, userFromRepo);
            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception($"Updating {id} faled on save");
        
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId ){

            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             {
                  return Unauthorized(); 
        
             }
             var like = await _repo.GetLike(id,recipientId);

             if (like != null)
             {
                 return BadRequest("Vec ste Like usera");
             }

             if (await _repo.GetUser(recipientId,false)==null)
             {
                 return NotFound();
             }
            like = new Like{
                LikerId = id,
                LekeeId = recipientId
            };

            _repo.Add<Like>(like);
            if (await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("Doslo je do greske pri lajkovanju");
        }

        // [HttpGet("{id}")]
        // private async Task<IActionResult> GetUser(int id)
        // {
        //     var user = await _repo.GetUser(2);
        //     return Ok(user);
        // }
    }
}