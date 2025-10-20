using Common.ViewModels;
using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<ResultViewModel<User>> GetByEmailAsync(string email);
    }
}
