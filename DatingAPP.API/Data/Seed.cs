using System.Collections.Generic;
using Newtonsoft.Json;
using DatingApp.API.Data;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace DatingAPP.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;


        }
        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                
            
                 var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                var roles = new List<Role>{

                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                    
                }



                 foreach (var user in users)
                {
                     _userManager.CreateAsync(user, "metan").Wait();
                     _userManager.AddToRoleAsync(user, "Member").Wait();
                     // Wait zato sto je asihrona metoda

                }

                var adminUser = new User{
                    UserName ="Admin"
                };
                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(admin, new[]{"Admin","Moderator"}).Wait();
                }
            }
        }


    }
}