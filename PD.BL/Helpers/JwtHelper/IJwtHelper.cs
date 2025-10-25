using PD.DAL.Entitites.AppEntitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Helpers.JwtHelper
{
    public interface IJwtHelper
    {
        string GenerateJwtToken(User user);
    }
}
