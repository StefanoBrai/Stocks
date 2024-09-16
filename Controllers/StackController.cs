using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProgettoWebAPI_Stocks.Data;
using ProgettoWebAPI_Stocks.DTOs.Stock;
using ProgettoWebAPI_Stocks.Interfaces;
using ProgettoWebAPI_Stocks.Mappers;

namespace ProgettoWebAPI_Stocks.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StackController : Controller
    {
        private readonly ILogger<StackController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepository;

        public StackController(ILogger<StackController> logger, ApplicationDbContext context, IStockRepository stockRepository)
        {
            _logger = logger;
            _context = context;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepository.GetAllAsync();
            var stockDTOs = stocks.Select(s => s.ToStockDTO());

            return Ok(stockDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stock.FindAsync(id);

            if(stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDTO createDTO)
        {
            var stockModel = createDTO.ToStockFromCreateStockDTO();

            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDTO updateDTO)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDTO.Symbol;
            stockModel.CompanyName = updateDTO.CompanyName;
            stockModel.Purchase = updateDTO.Purchase;
            stockModel.LastDiv = updateDTO.LastDiv;
            stockModel.Industry = updateDTO.Industry;
            stockModel.MarketCap = updateDTO.MarketCap;

            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}