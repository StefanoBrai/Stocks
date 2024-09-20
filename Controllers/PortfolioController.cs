using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProgettoWebAPI_Stocks.Extensions;
using ProgettoWebAPI_Stocks.Interfaces;
using ProgettoWebAPI_Stocks.Models;

namespace ProgettoWebAPI_Stocks.Controllers
{
    [Authorize]
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : Controller
    {
        private readonly ILogger<PortfolioController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(ILogger<PortfolioController> logger, UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio()
        {
            // Find user
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            // Get portfolio
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            
            return Ok(userPortfolio);
        }

        [HttpPost]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            // Find user
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            // Get stock
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                return BadRequest("Stock Not Found");
            }

            // Get portfolio
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot add same stock to portfolio");
            }

            // Create portfolio
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await _portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            // Find user
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            // Get portfolio
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            // Filter by symbol stock in portfolio we want to delete
            var fileredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());

            if (fileredStock.Count() == 1)
            {
                await _portfolioRepository.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your portfolio");
            }

            return Ok();
        }
    }
}