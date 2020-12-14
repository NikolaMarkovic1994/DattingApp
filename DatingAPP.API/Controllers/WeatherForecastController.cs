using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]// http://localhost:5000/WeatherForecast
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /*private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }*/

        private readonly DataContext _context;

        public WeatherForecastController(DataContext context )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vrednosti = await _context.Autos.ToListAsync();// povezivanje baze 
            //_context.Autos.ToList()
            return Ok(vrednosti);



        }
        /*public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }*/
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vrednosti = await _context.Autos.FindAsync(id);// povezivanje baze x=> x.id==id
            //_context.Autos.ToList()
            return Ok(vrednosti);



        }
        /*public IEnumerable<WeatherForecast> Get(int  id)
         {
             var rng = new Random();
             return Enumerable.Range(1, 5).Select(index => new WeatherForecast
             {
                 Date = DateTime.Now.AddDays(index),
                 TemperatureC = rng.Next(0, 10),
                 Summary = Summaries[rng.Next(Summaries.Length)]
             })
             .ToArray();
         }*/
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
