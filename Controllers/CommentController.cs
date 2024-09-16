using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProgettoWebAPI_Stocks.DTOs.Comment;
using ProgettoWebAPI_Stocks.Interfaces;
using ProgettoWebAPI_Stocks.Mappers;

namespace ProgettoWebAPI_Stocks.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ILogger<CommentController> logger, ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _logger = logger;
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepository.GetAllAsync();
            var commentDTOs = comments.Select(c => c.ToCommentDTO());

            return Ok(commentDTOs);
        }

        [HttpGet]
        [Route("{id:int})")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!await _stockRepository.StockExist(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = createDTO.ToCommentFromCreate(stockId);

            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepository.UpdateAsync(id, updateDTO.ToCommentFromUpdate());

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentModel = await _commentRepository.DeleteAsync(id);

            if (commentModel == null)
            {
                return NotFound("Comment does not exist");
            }

            return Ok(commentModel);
        }
    }
}