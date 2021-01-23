using System.Linq;
using AutoMapper;
using DatingAPP.API.Dtos;
using DatingAPP.API.Model;

namespace DatingAPP.API.Extensions
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User,UserForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>{
                opt.MapFrom(src =>src.Photos.FirstOrDefault(p =>p.IsMain).Url);
            })
            .ForMember(dest => dest.Age, opt =>{
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });
            CreateMap<User,UserForDetaileDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>{
                opt.MapFrom(src =>src.Photos.FirstOrDefault(p =>p.IsMain).Url);
            })
            .ForMember(dest => dest.Age, opt =>{
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            })
            ;
            CreateMap<Photo,PhotosForDeataledDto>();
            CreateMap<UserForUpadateDto,User>();
            CreateMap<Photo,PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto,Photo>();
            CreateMap<UserForRefDtos,User>();
            CreateMap<MessageForCreationDto,Message>().ReverseMap();
            CreateMap<MessageHubDto,Message>().ReverseMap();
             CreateMap<Message,MessageHubDto>();

             CreateMap<Message,MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt =>
                     opt.MapFrom(u =>u.Sender.Photos.FirstOrDefault(p =>p.IsMain).Url))
                .ForMember(m => m.RecepientPhotoUrl, opt =>
                     opt.MapFrom(u =>u.Recepient.Photos.FirstOrDefault(p =>p.IsMain).Url)
            );



        }
    }
}