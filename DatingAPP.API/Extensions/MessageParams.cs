namespace DatingAPP.API.Extensions
{
    public class MessageParams
    {
        private const int MaxPageSize =50;
        public int PageNumber { get; set; } =1;
        public int pageSize=10;
        public int PageSize   
        {   
            get { return pageSize; }
            set { pageSize= (value>MaxPageSize) ? MaxPageSize : value ; }
            // ako vrednost koju je trayio korisnik veca od 50 onda se vrace Mags page siza a ako ne vraga vrednost

        }
        // korisnik ne mora da postavlja svoje vrednosti za koliki broj korisnika zeli da vrati sa mogucnosti promene

        public int UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";
    }
}