using AutoMapper;
using Common.ViewModels;
using Dtos.UserDtos;
using FluentValidation;
using PD.DAL.Entitites.AppEntitites;
using PD.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.UserService
{

    public class UserService : IUserService
    {

        private readonly IBaseRepository<User> _userRepository;
        private readonly IUserRepository _userRepository2;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;
        private readonly IValidator<UpdatePasswordDto> _updatePasswordValidator;

        public UserService(
            IBaseRepository<User> userRepository,
            IMapper mapper,
            IValidator<RegisterDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator,
            IValidator<UpdatePasswordDto> updatePasswordValidator,
            IUserRepository userRepository2)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _updatePasswordValidator = updatePasswordValidator;
            _userRepository2 = userRepository2;
        }
        public async Task<ResultViewModel<UserDto>> CreateUserAsync(RegisterDto registerDto)
        {
            var validationResult = await _createUserValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return ResultViewModel<UserDto>.Failure("Girilen bilgileri kontrol et", errors, 400);
            }
            var existedEmail = await _userRepository2.GetByEmailAsync(registerDto.Email);
            if (existedEmail.Data != null)
            {
                return ResultViewModel<UserDto>.Failure("Bu email zaten kayıtlı", new List<string> { "Email zaten kullanılıyor" }, 400);
            }

            var userEntity = _mapper.Map<User>(registerDto);

            await _userRepository.AddAsync(userEntity);
            var userDto = _mapper.Map<UserDto>(userEntity);
            return ResultViewModel<UserDto>.Success(userDto, "Kullanıcı başarıyla oluşturuldu", 201);
        }

        public async Task<ResultViewModel<object>> DeleteUserAsync(int id)
        {
            var userid= await  _userRepository.GetByIdAsync(id);
            if (userid == null)
            {
                return await Task.FromResult(ResultViewModel<object>.NotFound("Kullanıcı bulunamadı", 404));
            }
            await _userRepository.DeleteAsync(userid);

            return ResultViewModel<object>.Success(null, "Kullanıcı başarıyla silindi", 200);
        }

        public async Task<ResultViewModel<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return ResultViewModel<List<UserDto>>.Success(userDtos, "Kullanıcılar başarıyla getirildi", 200);
        }
        

        public async Task<ResultViewModel<UserDto>> GetUserByIdAsync(int id)
        {
            var existedUser = await  _userRepository.GetByIdAsync(id);
            if (existedUser == null)
            {
                return await Task.FromResult(ResultViewModel<UserDto>.NotFound("Kullanıcı bulunamadı", 404));
            }
            var userDto = _mapper.Map<UserDto>(existedUser);
            return await Task.FromResult(ResultViewModel<UserDto>.Success(userDto, "Kullanıcı başarıyla getirildi", 200));
        }

        public async Task<ResultViewModel<object>> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto)
        {
            var existedUser = await GetUserByIdAsync(id);
            existedUser.Data.Password = updatePasswordDto.NewPassword;
            var userDto = _mapper.Map<UserDto>(existedUser);
            await _userRepository.UpdateAsync(_mapper.Map<User>(userDto));
            return ResultViewModel<object>.Success(null, "Şifre başarıyla güncellendi", 200);
        }

        public async Task<ResultViewModel<UserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var existedUser = await _userRepository.GetByIdAsync(id);
            if (existedUser == null)
            {
                return ResultViewModel<UserDto>.NotFound("Kullanıcı bulunamadı", 404);
            }
            _mapper.Map(updateUserDto, existedUser);
            await _userRepository.UpdateAsync(existedUser);
            var updatedUserDto = _mapper.Map<UserDto>(existedUser);
            return  ResultViewModel<UserDto>.Success(updatedUserDto, "Kullanıcı başarıyla güncellendi", 200);
        }
    }
}
