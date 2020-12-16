using System.Threading.Tasks;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;

using DatingApp.API.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DatingAPP.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]// http://localhost:5000/Auth
    public class AuthController : ControllerBase
    {
        private readonly IAutnRepository _repo;
        public AuthController(IAutnRepository repo)
        {
           _repo = repo;
        }
        [HttpPost("register")]//HttpPost za postavnjanje podataka u bazi
        public async Task<IActionResult> Register([FromBody]UserForRefDtos userForRefDtos)
        {
            userForRefDtos.UserName=userForRefDtos.UserName.ToLower();

            if (await _repo.UserExists(userForRefDtos.UserName))
            {
                return BadRequest("Username alreeady exists");
            }

            var userToCreate = new User
            {
                UserName = userForRefDtos.UserName
            };

            var userCreated = await _repo.Register(userToCreate,userForRefDtos.Pssword);

            return StatusCode(201);
        }

        

    }
}