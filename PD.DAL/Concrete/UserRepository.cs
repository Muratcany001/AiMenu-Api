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
        // Artık IBaseRepository<User> enjekte etmeye gerek yok. 
        // Temel sınıfa (BaseRepository) Context'i gönderiyoruz.
        public UserRepository(Context context)
            : base(context)
        {
            // Constructor'ın içi boş kalabilir veya sadece BaseRepository'ye Context'i göndermesi yeterlidir.
        }

        public async Task<ResultViewModel<User>> GetByEmailAsync(string email)
        {
            // ... (Kodunuz buraya devam eder, miras alınan _dbSet'i kullanır)
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
            return ResultViewModel<User>.Success(user, "kullanici bulundu", 200);
        }
    }
}
