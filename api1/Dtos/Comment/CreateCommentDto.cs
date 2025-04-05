using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api1.Dtos.Comment
{
    public class CreateCommentDto
    {
        // Adding Data Annotations for validation

        [Required]
        [MinLength(5, ErrorMessage = "Title must have atleast 5 chars")]
        [MaxLength(280, ErrorMessage = "Title is too long")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must have atleast 3 chars")]
        [MaxLength(280, ErrorMessage = "Content is too long")]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}