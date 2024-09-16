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
            var stock = await _stockRepository.GetByIdAsync(id);

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

            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDTO updateDTO)
        {
            var stockModel = await _stockRepository.UpdateAsync(id, updateDTO);

            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepository.DeleteAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}