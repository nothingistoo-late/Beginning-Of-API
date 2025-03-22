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
                                      .FirstOrDefaultAsync(od => od.Id == Rating.OrderDetailId);

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

            return Ok(Rating);
        }

        [HttpPost("likebyRatingid/{id}")]
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
        [HttpPost("dislikebyRatingid/{id}")]
        public async Task<IActionResult> DislikeRating(int id)
        {
            var rating = await _dbcontext.Ratings.FindAsync(id);
            if (rating == null)
                return NotFound("Rating not found.");

            rating.DislikeCount++;
            await _dbcontext.SaveChangesAsync();

            return Ok(new { rating.DislikeCount });
        }

        [HttpGet]
        [Route("getAllRating")]
        public async Task<IActionResult> GetAllRating()
        {
            return Ok(await _dbcontext.Ratings.Include(r=> r.Comments).ToListAsync());
        }

        [HttpDelete]
        [Route("deleteRatingByid/{id}")]
        public async Task<IActionResult> DeleteRatingById(int id)
        {
            try
            {
                // Tìm rating theo Id
                var rating = await _dbcontext.Ratings.FindAsync(id);

                // Nếu không tìm thấy
                if (rating == null)
                {
                    return NotFound(new
                    {
                        Message = $"Rating with ID {id} not found.",
                        Success = false
                    });
                }

                // Xóa rating
                _dbcontext.Ratings.Remove(rating);
                await _dbcontext.SaveChangesAsync();

                // Trả về thông báo thành công
                return Ok(new
                {
                    Message = $"Rating with ID {id} deleted successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                // Trả về lỗi chi tiết trong trường hợp exception
                return BadRequest(new
                {
                    Message = "An error occurred while deleting the rating.",
                    Success = false,
                    Error = ex.Message
                });
            }
        }


        [HttpPut]
        [Route("updateRatingBy{id}")]
        public async Task<IActionResult> updateRatingById(int id, [FromBody] UpdateRating obj )
        {
            var rating = await _dbcontext.Ratings
                      .FirstOrDefaultAsync(r => r.OrderDetailId == id);

            if (rating == null)
            {
                return NotFound("Rating not found for the given OrderDetail.");
            }

            if (obj.RatingValue > 5 || obj.RatingValue < 0)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            // Cập nhật giá trị của Rating
            rating.RatingValue = obj.RatingValue;
            rating.Comment = obj.Comment;

            _dbcontext.Ratings.Update(rating);
            await _dbcontext.SaveChangesAsync();

            return Ok(obj);
        }
    }

}
