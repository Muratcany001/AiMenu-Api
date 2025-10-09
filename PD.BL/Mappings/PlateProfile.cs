using AutoMapper;
using Dtos;
using Dtos.UserDtos;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Mappings
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
