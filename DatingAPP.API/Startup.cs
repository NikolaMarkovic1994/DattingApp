 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingAPP.API.Data;
using DatingAPP.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)// metod akodja koristi podesavanjea appsetings.json
        {
            Configuration = configuration;
        }
        /* 
        /// IConfiguration
        /// koristi opcije iz appsetings.json
        */ 
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // dependenci injektions
        public void ConfigureServices(IServiceCollection services)// metoda koja slucsi za dependcise
        {
            services.AddTransient<Seed>();
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            // povezivanje programa sa bazom koju koristimo

            /*services.AddDbContext<DataContext>(x => x.UseSqlite("Asdasda"));
             
            services- novi program ili usluga koja nam je potrebna za nas sajt
            .AddDbContext<Clasa koja u sebi sadrzi nazive tabela i ostale konfiguracije za bazu>
             x => x.UseSqlite("Asdasda") - koja koja se baza koisti

            Configuration.GetConnectionString("DefaultConnection"))
             Configuration omogucaca nam da pozovemo metodu iz appsettings.json
            GetConnectionString("DefaultConnection")) naziv metode koja se koristi
             */
           services.AddScoped<IAutnRepository,AutnRepository>();// pravi instancu klasce AR za svaki http zahtev
          services.AddScoped<IDatingRepository,DatingRepository>();
           // KONJINO  PAZI KAKO SI DAO NAZIVE KLASAMA!!!!!!!!!!!!
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuerSigningKey =true,
                    IssuerSigningKey =new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false

                
                };
            });
            services.AddCors();
            services.AddAutoMapper();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );;// omogucava koriscenje kontrolera Conrtoler Reset arhitektura
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DatingApp.API", Version = "v1" });
            });
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seed seder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DatingApp.API v1"));
                app.UseDeveloperExceptionPage();
            }else{

                app.UseExceptionHandler(builder => builder.Run(async context =>{context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                var error = context.Features.Get<IExceptionHandlerFeature>();
                if(error !=null)
                {
                    context.Response.AddApplocationError(error.Error.Message);
                    await context.Response.WriteAsync(error.Error.Message);
                }
                }));
            }

            //app.UseHttpsRedirection();

        //    seder.SeedUsers();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

           // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
         
           
        }
    }
}
