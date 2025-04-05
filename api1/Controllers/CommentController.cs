using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api1.Mappers;
using api1.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api1.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository _commentRepo;

        public CommentController(CommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
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

    }
}