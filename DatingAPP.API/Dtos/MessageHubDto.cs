namespace DatingAPP.API.Dtos
{
    public class MessageHubDto
    {
        
        public string user  { get; set; }
        public string Content  { get; set; }
        // public string msgText  { get; set; }


          public int SenderId { get; set; }
           public int RecipientId { get; set; }
    }
}