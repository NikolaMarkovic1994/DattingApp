using System.Collections.Generic;
using Newtonsoft.Json;
using DatingApp.API.Data;
using DatingAPP.API.Model;

namespace DatingAPP.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;

        }
        public void SeedUsers(){
        
        var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

        var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash,passwordSalt;
                CreatePassHash("password" , out passwordHash, out passwordSalt);
                user.PasswordHash=passwordHash;
                user.PasswordSalt=passwordSalt;
                user.UserName=user.UserName.ToLower();
                _context.Users.Add(user);

            }
            _context.SaveChanges();

        }

        private void CreatePassHash(string v, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                    
                     
                     
                    
                    // pomocu using metode instanca klase sys.security dje se porisati kada je nekoristo ova metoda
                passwordSalt= hmac.Key;// promenljivoj doda random koda
                passwordHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(v));
                //pretvara pasvorad u bite i enkriptuje pa vrsi enkripciju i cuva se kao ksa niz karaktera
            }
        }
    }
}