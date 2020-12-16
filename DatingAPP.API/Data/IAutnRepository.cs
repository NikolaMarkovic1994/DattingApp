using System.Threading.Tasks;
using DatingAPP.API.Model;

namespace DatingAPP.API.Data
{
    public interface IAutnRepository
    {
         Task<User> Register (User user, string password);

         Task<User> LogIn(string username, string password);

         Task<bool> UserExists(string user); 
    }
}