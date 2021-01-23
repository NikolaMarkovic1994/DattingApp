using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingAPP.API.Model
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}