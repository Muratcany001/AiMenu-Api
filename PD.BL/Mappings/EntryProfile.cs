using AutoMapper;
using Dtos.EntryDto;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Mappings
{
    public class EntryProfile : Profile
    {
        EntryProfile()
        {
            CreateMap<EntryDto, Entry>();
            CreateMap<Entry, EntryDto>();
        }
    }
}
