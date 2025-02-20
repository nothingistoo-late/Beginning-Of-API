using Final4.Data;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final4.Controllers
{
    public class CommentController : Controller
    {
        public ApplicationDBContext _dbcontext;
        public CommentController(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        // Thêm bình luận
        [HttpPost("addcommentbyid/{id}")]
        public async Task<IActionResult> AddComment(int id, [FromBody] string content)
        {
            var rating = await _dbcontext.Ratings.FindAsync(id);
            if (rating == null)
                return NotFound("Rating not found.");
            var comment = new Comment
            {
                RatingId = id,
                Content = content
            };
            _dbcontext.Comments.Add(comment);
            await _dbcontext.SaveChangesAsync();
            return Ok(comment);
        }

        // Lấy danh sách bình luận
        [HttpGet("getcommentByRatingId{id}")]
        public async Task<IActionResult> GetComments(int id)
        {
            var comments = await _dbcontext.Comments
                .Where(c => c.RatingId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Ok(comments);
        }
    }
}
