using Common.ViewModels;
using Dtos.UserDtos;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.UserService
{
    public interface IUserService
    {
        Task<ResultViewModel<UserDto>> CreateUserAsync(RegisterDto registerDto);
        Task<ResultViewModel<UserDto>> GetUserByIdAsync(int id);
        Task<ResultViewModel<List<UserDto>>> GetAllUsersAsync();
        Task<ResultViewModel<UserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<ResultViewModel<object>> DeleteUserAsync(int id);
        Task<ResultViewModel<object>> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto);
        
    }
}
