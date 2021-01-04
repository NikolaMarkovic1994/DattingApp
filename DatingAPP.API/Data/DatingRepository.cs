using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingAPP.API.Extensions;
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

        public async Task<Photo> GetPhoto(int id)
        {
            var photo= await _context.Photos.FirstOrDefaultAsync(p=>p.Id==id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user =  await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x=>x.Id==id);
            // .Include da vrati i slike korisnika iz tavele Photos
            return user;
        }

        public async Task<User> GetUserName(string userName)
        {
            var user =  await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x=>x.UserName==userName);
            // .Include da vrati i slike korisnika iz tavele Photos
            return user;        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
          var user =   _context.Users.Include(p => p.Photos).OrderByDescending(u =>u.DateOfBirth).AsQueryable();
          user = user.Where(u=> u.Id !=userParams.UserId);
          user = user.Where(u=> u.Gender ==userParams.Gender);

          if (userParams.MinAge != 18 || userParams.MaxAge != 90)
          {
              var minDob  = DateTime.Today.AddYears(-userParams.MaxAge - 1);
              var maxDob = DateTime.Today.AddYears(-userParams.MinAge );  
              user =  user.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth<= maxDob);         
          }

          if (!string.IsNullOrEmpty(userParams.OrderBy))
          {
              switch (userParams.OrderBy)
              {
                  case "created":
                    user = user.OrderByDescending(u => u.Created);
                    break;
                 default:
                   user = user.OrderByDescending(u => u.DateOfBirth);
                   break;

              }
          }

          return await PagedList<User>.CreateAsync(user, userParams.PageNumber,userParams.pageSize);

        }

        public async Task<Photo> MainPhotoFromUser(int id)
        {
            return await _context.Photos.Where(u=>u.UserId==id).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()> 0;
        }
    }
}