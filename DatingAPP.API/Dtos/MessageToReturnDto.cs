using System;
using DatingAPP.API.Model;

namespace DatingAPP.API.Dtos
{
    public class MessageToReturnDto
    {
           public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
         public string SenderUserName { get; set; }
        public string RecepientUserName { get; set; } 
        public string Content { get; set; }
        public bool IsRead { get; set; }
      
        public DateTime? DateRead { get; set; }
        public DateTime MessageSend { get; set; }

        public string  SenderPhotoUrl { get; set; }
        public string  RecepientPhotoUrl { get; set; }

    }
}