using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api1.Data;
using api1.Dtos.Comment;
using api1.Dtos.Stock;
using api1.Mappers;
using api1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api1.Repository
{
    public class CommentRepository
    {
        public readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetComments()
        {

            var comments = await _context.Comments.ToListAsync();

            return comments;
        }

        public async Task<Comment?> GetById(int id)
        {

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return null;

            return comment;
        }

        public async Task<Comment> Create(CreateCommentDto commentDto)
        {
            var commentModel = commentDto.ToCommentFromCommentDto();
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> Update(int id, UpdateCommentRequestDto updateDto)
        {

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return null;

            comment.Title = updateDto.Title;
            comment.Content = updateDto.Content;

            await _context.SaveChangesAsync();

            return comment;
        }
        public async Task<Comment?> Delete(int id)
        {

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return null;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;

        }
    }
}