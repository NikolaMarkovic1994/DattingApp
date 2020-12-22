using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingAPP.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingAPP.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            // ne paje upit babi vec se cuva u memori sve dok se neizvrsi
        }

        public void Delete<T>(T entity) where T : class
        {
           _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user =  await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x=>x.Id==id);
            // .Include da vrati i slike korisnika iz tavele Photos
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
          var user =  await _context.Users.Include(p => p.Photos).ToListAsync();
          return user;

        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()> 0;
        }
    }
}