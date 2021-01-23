using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Extensions;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DatingAPP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private Cloudinary _cloudinary;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly DataContext _dataContext;
        public AdminController(UserManager<User> userManager, DataContext dataContext, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _dataContext = dataContext;
            _cloudinaryConfig = cloudinaryConfig;
            _userManager = userManager;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret


            );
            _cloudinary = new Cloudinary(acc);

        }


        [Authorize(Policy = "ReguireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {

            var user = await _userManager.Users
            .Include(r => r.UserRoles)
            .ThenInclude(r => r.Role)
            .OrderBy(u => u.UserName)
            .Select(u => new
            {
                u.Id,
                Username = u.UserName,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            })
            .ToListAsync();

            return Ok(user);
        }
        [Authorize(Policy = "ReguireAdminRole")]
        [HttpPost("editRoles/{username}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectRoles = roleEditDto.RoleNames;

            selectRoles = selectRoles ?? new string[] { };
            var result = await _userManager.AddToRolesAsync(user, selectRoles.Except(userRoles));

            if (!result.Succeeded)
            {
                return BadRequest("Fale to add to roles");
            }
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectRoles));
            if (!result.Succeeded)
            {
                return BadRequest("Fale to remove the roles");
            }
            return Ok(await _userManager.GetRolesAsync(user));

        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosFromModeratiom")]
        public async Task<IActionResult> GetPhotoForModeration()
        {
            var photos =await _dataContext.Photos
            .Include(u => u.User)
            .IgnoreQueryFilters()
            .Where(p => p.IsApproved ==false)
            .Select(u => new {
                Id = u.Id,
                UserName = u.User.UserName,
                Url = u.Url,
                IsApproved = u.IsApproved
            }).ToListAsync();
            return Ok(photos);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approvePhoto/{photoId}")]
        public async Task<IActionResult> ApprovePhoto(int photoId){

             var photo =await _dataContext.Photos
            .IgnoreQueryFilters().FirstOrDefaultAsync(p =>p.Id ==photoId );
            
                photo.IsApproved =true;

                await _dataContext.SaveChangesAsync();
                return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("rejectPhoto/{photoId}")]
        public async Task<IActionResult> RejectPhoto(int photoId){

             var photo =await _dataContext.Photos
            .IgnoreQueryFilters().FirstOrDefaultAsync(p =>p.Id ==photoId );
            
               if (photo.IsMain)
               {
                   return BadRequest("You  cannot reject the mait photo");

               }
               if (photo.PublicId != null)
               {
                   var deletionParams = new DeletionParams(photo.PublicId);

               var deletionResult = _cloudinary.Destroy(deletionParams);

                //    var delate = new DeletionParams(photo.PublicId);

                //    var res = _cloudinary.Destroy(delate);
                   if (deletionResult.Result == "ok")
                   {
                       _dataContext.Photos.Remove(photo);
                   }
               }

               if(photo.PublicId == null)
               {
                   _dataContext.Photos.Remove(photo);
               }

                await _dataContext.SaveChangesAsync();
                return Ok();
        }



    }


}