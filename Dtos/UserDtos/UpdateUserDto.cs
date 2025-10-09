using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string Block { get; set; }
        public string Floor { get; set; }
        public string DoorNumber { get; set; }
        public string PhoneNumber { get; set; }
    
    }
}
