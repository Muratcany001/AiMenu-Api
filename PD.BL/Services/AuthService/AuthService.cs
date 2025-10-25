using Dtos.UserDtos;
using PD.BL.Helpers;
using PD.BL.Helpers.JwtHelper;
using PD.BL.Services.UserService;
using PD.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        private readonly HashHelper _hashHelper;
        
        public AuthService(
            IJwtHelper jwtHelper,
            HashHelper hashHelper,
            IUserRepository userRepository)
        {
            _jwtHelper = jwtHelper;
            _userRepository = userRepository;
            _hashHelper = hashHelper;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var existedUser = await _userRepository.GetSingleByConditionAsync(u => u.Email == loginDto.Email);
            if (existedUser == null)
            {
                return null;
            }

            if (!HashHelper.VerifyPasswordHash(loginDto.Password, existedUser.PasswordHash, existedUser.PasswordSalt))
            {
                return null;
            }

            
            return _jwtHelper.GenerateJwtToken(existedUser);
        }
    }

}
