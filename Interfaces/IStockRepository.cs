using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgettoWebAPI_Stocks.Models;

namespace ProgettoWebAPI_Stocks.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
    }
}