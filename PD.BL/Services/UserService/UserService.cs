using AutoMapper;
using Common.ViewModels;
using Dtos.UserDtos;
using FluentValidation;
using PD.BL.Helpers;
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
        private readonly HashHelper _hashHelper;
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
            HashHelper hashHelper,
            IUserRepository userRepository2)
        {
            _userRepository = userRepository;
            _hashHelper = hashHelper;
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
            var existedEmail = await _userRepository.GetSingleByConditionAsync(u => u.Email == registerDto.Email);
            if (existedEmail != null)
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
            var userEntity = await _userRepository.GetByIdAsync(id);
            if (userEntity == null)
            {
                return ResultViewModel<object>.Failure("Kullanıcı bulunamadı.", null, 404);
            }
            if (!HashHelper.VerifyPasswordHash(updatePasswordDto.OldPassword, userEntity.PasswordHash, userEntity.PasswordSalt))
            {
                return ResultViewModel<object>.Failure("Mevcut şifre yanlış.", null, 400);
            }            
            HashHelper.CreatePasswordHash(updatePasswordDto.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

            userEntity.PasswordHash = newPasswordHash;
            userEntity.PasswordSalt = newPasswordSalt;

            await _userRepository.UpdateAsync(userEntity);

            return ResultViewModel<object>.Success(null, "Şifre başarıyla güncellendi.", 200);
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
