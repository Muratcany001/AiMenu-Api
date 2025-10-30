using AutoMapper;
using Dtos.MenuItemDto;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Mappings
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItemDto, MenuItem>();
            CreateMap<AddMenuItemDto, MenuItem>();
            CreateMap<UpdateMenuItemDto, MenuItem>();

        }

    }
}
