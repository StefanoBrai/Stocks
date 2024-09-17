using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgettoWebAPI_Stocks.DTOs.Stock;
using ProgettoWebAPI_Stocks.Helpers;
using ProgettoWebAPI_Stocks.Models;

namespace ProgettoWebAPI_Stocks.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockDTO updateStockDTO);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExist(int id);
    }
}