namespace DatingAPP.API.Model
{
    public class Like
    {
        public int LikerId { get; set; }
        public int LekeeId { get; set; }
        public User Liker { get; set; }
        public User Likee { get; set; }
    }
}