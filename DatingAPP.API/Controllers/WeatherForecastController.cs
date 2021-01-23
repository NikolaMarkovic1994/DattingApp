using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]// http://localhost:5000/WeatherForecast
    public class WeatherForecastController : ControllerBase
    {
        private readonly IAutnRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public WeatherForecastController(IAutnRepository repo, IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }
        [HttpPost("reg")]//HttpPost za postavnjanje podataka u bazi
        public async Task<IActionResult> Register([FromBody] UserForRefDtos userForRefDtos)
        {
            
            
            var userToCreate = _mapper.Map<User>(userForRefDtos);
            var result = await _userManager.CreateAsync(userToCreate, userForRefDtos.Pssword);
           
            var userToReturn = _mapper.Map<UserForDetaileDto>(userToCreate);

            if (result.Succeeded)
            {
                 return CreatedAtRoute("GetUser", 
                     new {controller = "Users",id = userToCreate.Id}, userToReturn);
            }
           return BadRequest(result.Errors);
        }
        [HttpPost("log")]
        public async Task<IActionResult> LogIn([FromBody] UserForLogInDtos userForLogInDtos)
        {

            var user = await _userManager.FindByNameAsync(userForLogInDtos.Usernam);

            var result = await _signInManager.CheckPasswordSignInAsync(user,userForLogInDtos.Password, false);
            // proverava pasvord , a treci parametar sluzi za zakljucavanje nalova 
            // moze se podesiti da ako se otkuca odredjeni broj posresnih lozinki da dodje donzakljucavanja profila
          

          if (result.Succeeded)
          {
              
              var appUser = await _userManager.Users.Include(p =>p.Photos)
              .FirstOrDefaultAsync(u => u.NormalizedUserName == userForLogInDtos.Usernam.ToUpper());
               
                var userToReturn = _mapper.Map<UserForListDto>(appUser);

                  return Ok(new
            {

                token = GenerateJwtToken(appUser).Result,
                user=userToReturn

            });

          }
            return Unauthorized();

          
        }

         private async Task<string> GenerateJwtToken (User user){
          var clames = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.UserName)
             };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    clames.Add(new Claim(ClaimTypes.Role, role));
                }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            // VAZNO MORA SE PAZITI "AppSettings:Token" BEZ RAZMAKA
            // 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clames),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds

            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }



    }

   

    [ApiController]
    [Route("[controller]")]// http://localhost:500/WeatherForecast
    public class CenaController : ControllerBase
    {
        public ActionResult<string> Get()
        {

            return "Pozzz";
        }
    }
}
