using Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.AuthService
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto loginDto);
    }
}
