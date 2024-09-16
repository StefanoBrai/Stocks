using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgettoWebAPI_Stocks.Data;
using ProgettoWebAPI_Stocks.DTOs.Stock;
using ProgettoWebAPI_Stocks.Interfaces;
using ProgettoWebAPI_Stocks.Models;

namespace ProgettoWebAPI_Stocks.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            
            if (stockModel == null) 
            {
                return null;
            }

            _context.Stock.Remove(stockModel);
            _context.SaveChanges();

            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stock.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> StockExist(int id)
        {
            return await _context.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockDTO updateStockDTO)
        {
            var existingStockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStockModel == null)
            {
                return null;
            }

            existingStockModel.Symbol = updateStockDTO.Symbol;
            existingStockModel.CompanyName = updateStockDTO.CompanyName;
            existingStockModel.Purchase = updateStockDTO.Purchase;
            existingStockModel.LastDiv = updateStockDTO.LastDiv;
            existingStockModel.Industry = updateStockDTO.Industry;
            existingStockModel.MarketCap = updateStockDTO.MarketCap;

            await _context.SaveChangesAsync();

            return existingStockModel;
        }
    }
}