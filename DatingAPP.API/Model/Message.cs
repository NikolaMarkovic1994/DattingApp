using System;

namespace DatingAPP.API.Model
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public User Sender { get; set; }
        public User Recepient { get; set; } 
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecepientDeleted { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSend { get; set; }
        
    }
}