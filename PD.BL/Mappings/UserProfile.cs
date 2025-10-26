using AutoMapper;
using Dtos.UserDtos;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {     
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UpdateUserDto>();
            CreateMap<UpdatePasswordDto, User>();
            CreateMap<User, UpdatePasswordDto>();
            CreateMap<RegisterDto, User>();
            CreateMap<User, RegisterDto>();
            CreateMap<LoginDto, User>();
        }
    }
}
