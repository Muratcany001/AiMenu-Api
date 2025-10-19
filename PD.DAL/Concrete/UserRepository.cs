using Common.ViewModels;
using Microsoft.EntityFrameworkCore;
using PD.DAL.Entitites.AppEntitites;
using PD.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<User> _baseRepository;

        // Fix for CS7036: Add required 'Context context' parameter and pass to base constructor
        public UserRepository(Context context, IUserRepository userRepository, IBaseRepository<User> baseRepository)
            : base(context)
        {
            _userRepository = userRepository;
            _baseRepository = baseRepository;
        }

        public async Task<ResultViewModel<User>> GetByEmailAsync(string email)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
            return ResultViewModel<User>.Success(user, "kullanici bulundu", 200);
        }
    }
}
