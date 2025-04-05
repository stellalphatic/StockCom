using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api1.Dtos.Comment;
using api1.Mappers;
using api1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace api1.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository _commentRepo;
        private readonly StockRepository _stockRepo;

        public CommentController(CommentRepository commentRepo, StockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            var comment = await _commentRepo.GetById(id);
            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var comment = await _commentRepo.GetComments();
            var commentDtos = comment.Select(x => x.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto comment)
        {
            bool exist = await _stockRepo.isStockExist(stockId);
            if (!exist)
                return BadRequest("Stock doesn't exist");
            var commentModel = await _commentRepo.Create(comment);

            return Ok(commentModel.ToCommentDto());

        }

    }
}