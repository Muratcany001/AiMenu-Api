using AutoMapper;
using Common.ViewModels;
using Dtos.UserDtos;
using FluentValidation;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.UserService
{

    public class UserService : IUserService
    {

        private readonly IUserService _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;
        private readonly IValidator<UpdatePasswordDto> _updatePasswordValidator;

        public UserService(
            IUserService userRepository,
            IMapper mapper,
            IValidator<RegisterDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator,
            IValidator<UpdatePasswordDto> updatePasswordValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _updatePasswordValidator = updatePasswordValidator;
        }
        public async Task<ResultViewModel<UserDto>> CreateUserAsync(RegisterDto registerDto)
        {
            var validationResult = await _createUserValidator.ValidateAsync(registerDto);           
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return ResultViewModel<UserDto>.Failure("Girilen bilgileri kontrol et", errors, 400);
            }
            var isEmailExists= await _userRepository.GetByEmailAsync(registerDto.Email);

            if (isEmailExists != null)
            {
                return ResultViewModel<UserDto>.Failure("Bu email zaten kayıtlı", null, 400);
            }
            var userEntity = _mapper.Map<User>(registerDto);
            
            await _userRepository.CreateUserAsync(registerDto);
            var userDto = _mapper.Map<UserDto>(userEntity);
            return ResultViewModel<UserDto>.Success(userDto, "Kullanıcı başarıyla oluşturuldu", 201);
        }

        public async Task<ResultViewModel<object>> DeleteUserAsync(int id)
        {
            var userid= _userRepository.GetUserByIdAsync(id);
            if (userid == null || !userid.Result.IsSuccess)
            {
                return await Task.FromResult(ResultViewModel<object>.NotFound("Kullanıcı bulunamadı", 404));
            }
            await _userRepository.DeleteUserAsync(id);

            return ResultViewModel<object>.Success(null, "Kullanıcı başarıyla silindi", 200);
        }

        public Task<ResultViewModel<List<UserDto>>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultViewModel<User>> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ResultViewModel<UserDto>> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultViewModel<object>> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultViewModel<UserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
