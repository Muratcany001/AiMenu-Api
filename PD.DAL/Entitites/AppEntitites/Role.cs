using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.DAL.Entitites.AppEntitites
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; } //Admin User
        public virtual ICollection<User> Users { get; set; } //Bu role sahip kullanıcılar listesi
    }
}
