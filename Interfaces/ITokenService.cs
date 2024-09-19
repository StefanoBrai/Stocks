using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgettoWebAPI_Stocks.Models;

namespace ProgettoWebAPI_Stocks.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}