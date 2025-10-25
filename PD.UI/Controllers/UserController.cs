using Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using PD.BL.Services.AuthService;
using PD.BL.Services.UserService;
using PD.DAL.Interface;

namespace PD.UI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IUserRepository userRepository, IAuthService authService)
        {
            _userService = userService;
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("api/users/login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _authService.Login(loginDto);
            return Ok(new { token = token });
        }

        [HttpPost("api/users/createUser")]
        public async Task<IActionResult> CreateUser(RegisterDto createUserDto)
        {
            var createdUser = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser }, createdUser);
        }


        [HttpGet("api/users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }
        [HttpGet("api/users/getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpPut("api/users/updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(updatedUser);
        }
        [HttpDelete("api/users/deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return Ok(result);
        }
        [HttpPut("api/users/updatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, UpdatePasswordDto updatePasswordDto)
        {
            var result = await _userService.UpdatePasswordAsync(id, updatePasswordDto);
            return Ok(result);
        }
    }
}
