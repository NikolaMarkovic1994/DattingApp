using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingAPP.API.Model
{
    public class User : IdentityUser<int>
    {
        // public int Id { get; set; }   

        // public string UserName { get; set; }

        // public byte[] PasswordHash  { get;set; }
        // public byte[] PasswordSalt { get; set; }

        // ova svojstva vec postoje u klasi IdentitiyUser pa ih nije potrebno ponovo postavljati
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Interests { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public ICollection<Photo> Photos  { get; set; }
        // User je povezan sa tabelom photos

        public ICollection<Like> Likers { get; set; }
        public ICollection<Like> Likees { get; set; }

        public ICollection<Message> MessagesSend { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }
        
}