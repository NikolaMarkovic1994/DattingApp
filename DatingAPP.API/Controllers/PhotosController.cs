using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
using DatingAPP.API.Extensions;
using DatingAPP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingAPP.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("users/{userid}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings;
            _mapper = mapper;
            _repo = repo;
            Account acc = new Account(
            _cloudinarySettings.Value.CloudName,
            _cloudinarySettings.Value.ApiKey,
            _cloudinarySettings.Value.ApiSecret
            );
            
            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("/{id}", Name = "GetPhoto")]
        
        public async Task<IActionResult> GetPhoto(int id){
            /*VAZNO MORA SE STAVITI / KOD {ID}*/
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userid,[FromForm] PhotoForCreationDto photoForCreationDto){

            var userFromRepo = await _repo.GetUser(userid);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length>0)
            {
                using(var stream = file.OpenReadStream())//
                {  
                    var uploadParams = new ImageUploadParams(){
                    File = new FileDescription(file.Name,stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };  
                    uploadResult = _cloudinary.Upload(uploadParams);             
                }
            }
             photoForCreationDto.Url = uploadResult.Uri.ToString();
             photoForCreationDto.PublicId=uploadResult.PublicId;
             
             var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u =>u.IsMain))
            {
                photo.IsMain=true;
            }

            userFromRepo.Photos.Add(photo);
            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id=photo.Id},photoToReturn);
            }
            return BadRequest("BRHA PHOTO NOT ADDED");
        }
        [HttpPost("{id}/ismain")]
        public async Task<IActionResult> SetMainPhoto(int userid,int id){

            var user =await _repo.GetUser(userid);
            if (!user.Photos.Any(p=>p.Id==id))
            {
                return BadRequest("Slika nije pronadjena");
            }

                var photoFromRepo=await _repo.GetPhoto(id);
                if (photoFromRepo.IsMain)
                {
                  return BadRequest("Ova fotografija je vec glavna");

                }
                var curentmainPhoto= await _repo.MainPhotoFromUser(userid);
                curentmainPhoto.IsMain=false;
                photoFromRepo.IsMain=true;
                if (await _repo.SaveAll())
                {
                    return NoContent();
                }
                return BadRequest("Fotografija nije postavljena");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id, int userid){
            
            var user =await _repo.GetUser(userid);
            if (!user.Photos.Any(p=>p.Id==id))
            {
                return BadRequest("Slika nije pronadjena");
            }

                var photoFromRepo=await _repo.GetPhoto(id);
                if (photoFromRepo.IsMain)
                {
                  return BadRequest("Nije moguce obrisati sliku");

                }
            
            if (photoFromRepo.PublicId!= null)
            {
                var deleteParams =new DeletionParams(photoFromRepo.PublicId);
                var rezult = _cloudinary.Destroy(deleteParams);
                 if (rezult.Result=="ok")
                    {
                        _repo.Delete(photoFromRepo);
                    }
            }
            if (photoFromRepo.PublicId == null)
            {
                _repo.Delete(photoFromRepo);
                   
            }
           
            if (await _repo.SaveAll())
            {
                return Ok();
            }
          return BadRequest("Neka breska brt");
        }
    }
}