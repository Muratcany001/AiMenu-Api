using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.UserDtos
{
    public class UpdatePasswordDto
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; } 
        public string OldPassword { get; set; }
    }
}
