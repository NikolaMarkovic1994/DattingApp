using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingAPP.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
namespace DatingAPP.API.Extensions
{
    public class LogUserActiv : IAsyncActionFilter
    {
       public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            // var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userId = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            System.Console.WriteLine(userId);


            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();
            // mozemo da kreirama iDating interfejs

            //var user =await repo.GetUser(userId);
            //user.LastActive = DateTime.Now;
           // await repo.SaveAll();
        }

    }
}