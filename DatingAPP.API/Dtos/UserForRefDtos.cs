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
    }
}