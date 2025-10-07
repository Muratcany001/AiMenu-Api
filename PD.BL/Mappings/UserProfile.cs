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
        }
    }
}
