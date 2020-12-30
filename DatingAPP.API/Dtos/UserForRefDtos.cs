using System;
using System.ComponentModel.DataAnnotations;

namespace DatingAPP.API.Dtos
{
    public class UserForRefDtos
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage= "Losinka mora biti izmedju 4 do 8 karaktera")]
        public  string Pssword { get; set; }
       [Required]
        public string City { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        public DateTime Created { get; set; }

         public DateTime LastActive { get; set; }
         public UserForRefDtos()
         {
             Created=DateTime.Now;
             LastActive=DateTime.Now;
         }



      
      
      
     
    }
}