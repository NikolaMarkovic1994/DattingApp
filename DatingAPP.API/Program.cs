using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program{
  
        // galcna klasa 
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();// pokretanje web aplikacije
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();// pokretanje klase koja u sebi strzi podesavanja i dependensi
                });
    }
}
