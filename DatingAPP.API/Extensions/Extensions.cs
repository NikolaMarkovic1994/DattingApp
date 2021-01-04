using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
         public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var pagHeader = new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(pagHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
            response.Headers.Add("Access-Control-Allow-Origins", "*");

        } 
        public static int CalculateAge(this DateTime theDateTime){
            
            var age = DateTime.Today.Year - theDateTime.Year;
            if(theDateTime.AddYears(age)>DateTime.Today)
            {
                age --;
            }
            return age;
        }

    }

}