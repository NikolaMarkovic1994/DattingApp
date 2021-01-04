using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAPP.API.Extensions;
using DatingAPP.API.Model;

namespace DatingAPP.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<User> GetUserName(string userName);
         Task<Photo> GetPhoto(int id);
         Task<Photo> MainPhotoFromUser(int id);

    }
}