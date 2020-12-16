using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Model;
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
        // private static readonly string[] Summaries = new[]
        // {
        //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        // };

        // /*private readonly ILogger<WeatherForecastController> _logger;

        // public WeatherForecastController(ILogger<WeatherForecastController> logger)
        // {
        //     _logger = logger;
        // }*/

        // private readonly DataContext _context;

        // public WeatherForecastController(DataContext context )
        // {
        //     _context = context;
        // }

        // [HttpGet]
        // public async Task<IActionResult> Get()
        // {
        //     var vrednosti = await _context.Autos.ToListAsync();// povezivanje baze 
        //     //_context.Autos.ToList()
        //     return Ok(vrednosti);



        // }
        // /*public IEnumerable<WeatherForecast> Get()
        // {
        //     var rng = new Random();
        //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //     {
        //         Date = DateTime.Now.AddDays(index),
        //         TemperatureC = rng.Next(-20, 55),
        //         Summary = Summaries[rng.Next(Summaries.Length)]
        //     })
        //     .ToArray();
        // }*/
        // [HttpGet("{id}")]
        // public async Task<IActionResult> Get(int id)
        // {
        //     var vrednosti = await _context.Autos.FindAsync(id);// povezivanje baze x=> x.id==id
        //     //_context.Autos.ToList()
        //     return Ok(vrednosti);



        // }
        // /*public IEnumerable<WeatherForecast> Get(int  id)
        //  {
        //      var rng = new Random();
        //      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //      {
        //          Date = DateTime.Now.AddDays(index),
        //          TemperatureC = rng.Next(0, 10),
        //          Summary = Summaries[rng.Next(Summaries.Length)]
        //      })
        //      .ToArray();
        //  }*/
        private readonly IAutnRepository _repo;
        private readonly IConfiguration _config;
        public WeatherForecastController(IAutnRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost("reg")]//HttpPost za postavnjanje podataka u bazi
        public async Task<IActionResult> Register([FromBody] UserForRefDtos userForRefDtos)
        {
            userForRefDtos.UserName = userForRefDtos.UserName.ToLower();

            if (await _repo.UserExists(userForRefDtos.UserName))
            {
                return BadRequest("Username alreeady exists");
            }

            var userToCreate = new User
            {
                UserName = userForRefDtos.UserName
            };

            var userCreated = await _repo.Register(userToCreate, userForRefDtos.Pssword);

            return StatusCode(201);
        }
        [HttpPost("log")]
        public async Task<IActionResult> LogIn([FromBody] UserForLogInDtos userForLogInDtos)
        {
            var userFromDb = await _repo.LogIn(userForLogInDtos.Usernam.ToLower(), userForLogInDtos.Password);
            // promenljivoj userFromDb popenjuje korisnika iz baze ako je sve uredu

            if (userFromDb == null)
            {
                return Unauthorized();
            }
            var clames = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                 new Claim(ClaimTypes.Name, userFromDb.UserName)
             };

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

             return Ok( new {

                 token = tokenHandler.WriteToken(token)

             });
        }

    }

    [ApiController]
    [Route("[controller]")]// http://localhost:500/WeatherForecast
    public class CenaController : ControllerBase
    {
        public ActionResult<string> Get()
        {

            return "Kurcina2";
        }
    }
}
