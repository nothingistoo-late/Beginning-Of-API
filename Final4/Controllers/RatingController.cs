using Final4.Data;
using Final4.DTO.Rating;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final4.Controllers
{
    public class RatingController : Controller
    {
        public ApplicationDBContext _dbcontext;
        public RatingController(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost]
        [Route("addRating")]
        public async Task<IActionResult> AddRating([FromBody] AddRating Rating)
        {
            var orderDetail = await _dbcontext.OrderDetails
                                      .Include(od => od.Flower)
                                      .FirstOrDefaultAsync(od => od.OrderDetailId == Rating.OrderDetailId);

            if (orderDetail == null)
            {
                return BadRequest("OrderDetail not found.");
            }

            if (Rating.RatingValue > 5 || Rating.RatingValue < 0)
                return BadRequest("Rating must be between 1 and 5.");

            var rating = new Rating
            {
                OrderDetailId = Rating.OrderDetailId,
                RatingValue = Rating.RatingValue,
                Comment = Rating.Comment
            };

            _dbcontext.Ratings.Add(rating);
            await _dbcontext.SaveChangesAsync();

            return Ok(rating);
        }

        [HttpPost("{id}/likebyid")]
        public async Task<IActionResult> LikeRating(int id)
        {
            var rating = await _dbcontext.Ratings.FindAsync(id);
            if (rating == null)
                return NotFound("Rating not found.");

            rating.LikeCount++;
            await _dbcontext.SaveChangesAsync();

            return Ok(new { rating.LikeCount });
        }

        // Tăng Dislike
        [HttpPost("{id}/dislikebyid")]
        public async Task<IActionResult> DislikeRating(int id)
        {
            var rating = await _dbcontext.Ratings.FindAsync(id);
            if (rating == null)
                return NotFound("Rating not found.");

            rating.DislikeCount++;
            await _dbcontext.SaveChangesAsync();

            return Ok(new { rating.DislikeCount });
        }

        // Thêm bình luận
        [HttpPost("{id}/addcomment")]
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
        [HttpGet("{id}/getcomment")]
        public async Task<IActionResult> GetComments(int id)
        {
            var comments = await _dbcontext.Comments
                .Where(c => c.RatingId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Ok(comments);
        }

        [HttpGet]
        [Route("getAllRating")]
        public async Task<IActionResult> GetAllRating()
        {
            return Ok(await _dbcontext.Ratings.Include(r=> r.Comments).ToListAsync());
        }
    }

}
