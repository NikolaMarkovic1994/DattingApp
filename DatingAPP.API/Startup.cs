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
using DatingAPP.API.Model;
using DatingAPP.API.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
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
//         public void ConfigureServices(IServiceCollection services)// metoda koja slucsi za dependcise
//         {
//             services.AddTransient<Seed>();
//             services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
//             services.AddDbContext<DataContext>(x =>
//              x.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
//             // povezivanje programa sa bazom koju koristimo

//             /*services.AddDbContext<DataContext>(x => x.UseSqlite("Asdasda"));
//              "DefaultConnection": "server=localhost;port=3306; database=datingApp; uid=appuser; pwd=4 Jul !994"
//              //   "ConnectionStrings": {
// //     "DefaultConnection": "Data Source=DatingApp.Db"
// // }
//             services- novi program ili usluga koja nam je potrebna za nas sajt
//             .AddDbContext<Clasa koja u sebi sadrzi nazive tabela i ostale konfiguracije za bazu>
//              x => x.UseSqlite("Asdasda") - koja koja se baza koisti

//             Configuration.GetConnectionString("DefaultConnection"))
//              Configuration omogucaca nam da pozovemo metodu iz appsettings.json
//             GetConnectionString("DefaultConnection")) naziv metode koja se koristi
//              */
//            services.AddScoped<IAutnRepository,AutnRepository>();// pravi instancu klasce AR za svaki http zahtev
//           services.AddScoped<IDatingRepository,DatingRepository>();
//            // KONJINO  PAZI KAKO SI DAO NAZIVE KLASAMA!!!!!!!!!!!!
//             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
//                 options.TokenValidationParameters = new TokenValidationParameters 
//                 {
//                     ValidateIssuerSigningKey =true,
//                     IssuerSigningKey =new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
//                     ValidateIssuer = false,
//                     ValidateAudience = false

                
//                 };
//             });
//             services.AddCors();
//             services.AddAutoMapper();
//             services.AddControllers().AddNewtonsoftJson(options =>
//                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//                 );;/* posto su klase photo i user povezane (postije parametri u obe metode koje su tipa user ili photo)
//                   pri http requestu server se odgovoriti kao petlju tj poslace prvog usera i unautar njega njegove fotogracije,
//                   a posto unutar klase Photo postoji klas User unutar odgovora za korisnikove fpotogracije pobovo ce poslati
//                   podatke o korisniu itd pvim opcijamo zaustavljamo to
//                 */
//             services.AddSwaggerGen(c =>
//             {
//                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "DatingApp.API", Version = "v1" });
//             });
//           services.AddScoped<LogUserActiv>();
//         }
        /*
        
        *********************
        
        */

         public void ConfigureServices(IServiceCollection services)// metoda koja slucsi za dependcise
        {
            services.AddSignalR();
            services.AddTransient<Seed>();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
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
            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit= false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric= false;
                opt.Password.RequireUppercase= false;
            }
            ).AddRoles<Role>()
            .AddEntityFrameworkStores<DataContext>()
             .AddRoleValidator<RoleValidator<Role>>()
             .AddRoleManager<RoleManager<Role>>()
             .AddSignInManager<SignInManager<User>>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuerSigningKey =true,
                    IssuerSigningKey =new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false

                
                };
                options.Events =new JwtBearerEvents{
                    OnMessageReceived = context => {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                            
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>{
                options.AddPolicy("ReguireAdminRole", policy => policy.RequireRole("Admin") );
                options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin","Moderator") );
                options.AddPolicy("VipOnle", policy => policy.RequireRole("VIP") );

            });
            /*
            
            Postebno je definisati zato seto smo deklaarisali BUILDER kao AddIdentityCore
            a dasmo koristili AddIdentety onda ovo nije potrebno ali bi smo bili prinudjeni
            da koristimo cookies
            
            */


            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IAutnRepository,AutnRepository>();// pravi instancu klasce AR za svaki http zahtev
            services.AddScoped<IDatingRepository,DatingRepository>();
           // KONJINO  PAZI KAKO SI DAO NAZIVE KLASAMA!!!!!!!!!!!!
            
            services.AddCors();
            Mapper.Reset();// pri brisanjeu baze kod SQllite pravi gresku pa se dodaje ovaj kod
            services.AddAutoMapper();
            /*
              services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Register other policies here
        }); Koristi se kao globalni Autorize kako nebi kitili savaki kontroler sa autorize mora se dalje pogledai kako radi u novim verzijema
            
            
            
            */
            services.AddSignalR();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );;/* posto su klase photo i user povezane (postije parametri u obe metode koje su tipa user ili photo)
                  pri http requestu server se odgovoriti kao petlju tj poslace prvog usera i unautar njega njegove fotogracije,
                  a posto unutar klase Photo postoji klas User unutar odgovora za korisnikove fpotogracije pobovo ce poslati
                  podatke o korisniu itd pvim opcijamo zaustavljamo to
                */
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DatingApp.API", Version = "v1" });
            });
          services.AddScoped<LogUserActiv>();
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

           // app.UseHttpsRedirection();

            // seder.SeedUsers();// ubacivanje korisnika u bazu
            app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials());


             app.UseDefaultFiles(); 
           // da u wwwroot pokecese index fajl ili neki dr default jajl

            app.UseStaticFiles();
            // omogucava kori[cenje staticnih fajlova tj ide wwwroot folder i pokrece njih
            // MORA SE POSTAVITI PRE AUTHENTICATION I AUTHORIZATION I VEROVATNO ROUTING
            //  OVDE JE VEOMA VAZAN RASPORED
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
           
            // omogucava kori[cenje staticnih fajlova tj ide wwwroot folder i pokrece njih
            app.UseEndpoints(endpoints =>
            {
                 endpoints.MapFallbackToController("Index", "Fallback");
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapHub<MessageHub>("hubs/messages");
                endpoints.MapHub<MessageTestHub>("hubs/messages-test");
                endpoints.MapHub<EchoHub>("hubs/echo");


                // hubs/presence pomocu kojega pozivamo PresenceHub
             
                // govori aplikaciji da koristi angular route 
            });
            // app.UseMvc(routs =>{
            //     routs.MapSpaFallbackRoute(
            //         name: "spa-fallback"
            //         default: new {conteroler = "Fallback" , action = "Index"}
            //     );
            // });
           
        }
    }
}
