using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgettoWebAPI_Stocks.DTOs.Stock;
using ProgettoWebAPI_Stocks.Models;

namespace ProgettoWebAPI_Stocks.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDTO()).ToList()
            };
        }

        public static Stock ToStockFromCreateStockDTO(this CreateStockDTO createStockRequestDTO)
        {
            return new Stock
            {
                Symbol = createStockRequestDTO.Symbol,
                CompanyName = createStockRequestDTO.CompanyName,
                Purchase = createStockRequestDTO.Purchase,
                LastDiv = createStockRequestDTO.LastDiv,
                Industry = createStockRequestDTO.Industry,
                MarketCap = createStockRequestDTO.MarketCap
            };
        }
    }
}