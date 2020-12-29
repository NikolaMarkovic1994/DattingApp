using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPP.API.Controllers
{
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
        public async Task<IActionResult> GetUsers()
        {

            var users = await _repo.GetUsers();
            var usersToReturn= _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers1(int id)
        {

            // var users = await _repo.GetUsers(); 
            // return Ok(users);
            var user = await _repo.GetUser(id);
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
             

            var userFromRepo = await _repo.GetUser(id);
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

        // [HttpGet("{id}")]
        // private async Task<IActionResult> GetUser(int id)
        // {
        //     var user = await _repo.GetUser(2);
        //     return Ok(user);
        // }
    }
}