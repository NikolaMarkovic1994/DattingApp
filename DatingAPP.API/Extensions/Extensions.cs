using Microsoft.AspNetCore.Http;

namespace DatingAPP.API.Extensions
{
    public static class Extensions
    {
        public static void AddApplocationError(this HttpResponse response, string Message)
        {
            response.Headers.Add("Application-Error", Message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origins", "*");

        } 
    }
}