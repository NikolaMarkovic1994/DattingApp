using System;

namespace DatingAPP.API.Model
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public  string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        public bool IsApproved { get; set; }


        // Postavnjamo opcije kako bi pri brisanju korisnika obisali i njegove fotografije
        public User User { get; set; }
        public int UserId { get; set; }
    }
}