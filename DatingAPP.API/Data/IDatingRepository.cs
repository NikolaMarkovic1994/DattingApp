using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAPP.API.Extensions;
using DatingAPP.API.Model;

namespace DatingAPP.API.Data
{
    public interface IDatingRepository
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connectionId);
        Task<Connection> GetConnection (string connectionId);
        Task<Group> GetMessageGroup(string groupName);

        Task<Group> GetGroupForConnection(string connectionId);
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id, bool isCurrentUser);
         Task<User> GetUserName(string userName);
         Task<Photo> GetPhoto(int id);
         Task<Photo> MainPhotoFromUser(int id);
         Task <Like> GetLike(int userId, int recipientId);

         Task<Message> GetMessage (int id);
         Task<PagedList<Message>> GetMessagesFromUser (MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessageThread (int userId, int recipientId);

    }
}