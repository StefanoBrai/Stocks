using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgettoWebAPI_Stocks.Data;
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
        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stock.ToListAsync();
        }
    }
}