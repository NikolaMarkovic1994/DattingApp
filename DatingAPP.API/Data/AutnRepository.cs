using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingAPP.API.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;// VAZHO!!!!!!!

namespace DatingAPP.API.Data
{
    public class AutnRepository : IAutnRepository
    {
        private readonly DataContext _context ; // povezivanje intervejca sa Data Contexst uvek je potrebno ako se radi sa bazom
        public AutnRepository(DataContext context)
        {
            _context = context;

        }
        
        public async Task<User> LogIn(string username, string password)
        {
            
            // Ansihron metoda vazno
            var user =await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);// ide u bazu i trazi korisnika sa istim korisnickim imenom
            if(username==null)
                 return null;
            if(!VerPassHash(password, user.PasswordHash,user.PasswordSalt))
                return null;
           return user;     
        }

        private bool VerPassHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                    
               
                var newHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < newHash.Length; i++)
                {
                    if (newHash[i] !=  passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
                
            }
        }

        public async Task<User> Register(User user, string password)
        {
            //async petoda ansihroda omaogucava da baza prima upite i ovaj upit ce cekati sve dok baza mu neda rezultat
            byte[] passHash, passSalt;
            //CreatePassHash(password, out passHash, out passSalt);// out za prenos reference ne vrednosti

            //  user.PasswordHash=passHash;
            //  user.PasswordSalt=passSalt;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("passHash");
             byte[] bytes1 = System.Text.Encoding.UTF8.GetBytes("passSalt");
             byte[] bytes3 = System.Text.Encoding.UTF8.GetBytes(password); 
             CreatePassHash(bytes3 , out passHash, out passSalt);
            user.PasswordHash=passHash;
             user.PasswordSalt=passSalt;

             await _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();// ubacivanje usra u bazu

            return user;
        }

        private void CreatePassHash(byte[] bytes3 , out byte[] passHash, out byte[] passSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                    
                     
                     
                    
                    // pomocu using metode instanca klase sys.security dje se porisati kada je nekoristo ova metoda
                passSalt= hmac.Key;// promenljivoj doda random koda
                passHash= hmac.ComputeHash(bytes3);
                //pretvara pasvorad u bite i enkriptuje pa vrsi enkripciju i cuva se kao ksa niz karaktera
            }
                // var hmac = new System.Security.Cryptography.HMACSHA512();// instanciranje klase 
                // klasea koja je odgovoran sa hasovanje lozinke to jest enkriptovanje lozinke
        }

        public async Task<bool> UserExists(string user)
        {
            
            
            
            if (await _context.Users.AnyAsync(x => x.UserName == user))
            {
                return true;
            }
            return false;
        }
    }
}