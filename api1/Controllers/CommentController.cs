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
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetById(id);
            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetComments();
            var commentDtos = comment.Select(x => x.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool exist = await _stockRepo.isStockExist(stockId);
            if (!exist)
                return BadRequest("Stock doesn't exist");
            var commentModel = await _commentRepo.Create(comment, stockId);

            return Ok(commentModel.ToCommentDto());

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateComment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _commentRepo.Update(id, updateComment);
            if (commentModel == null)
                return NotFound("Comment not found");


            return Ok(commentModel);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _stockRepo.Delete(id);
            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

    }
}