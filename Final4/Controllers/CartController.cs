using Final4.Data;
using Final4.DTO.Cart;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final4.Controllers
{
    public class CartController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _dbContext;
        private readonly EmailService _emailService;
        public CartController(ApplicationDBContext DBContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = DBContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpGet("my-cart")]
        [Authorize]
        public async Task<IActionResult> GetMyCart()
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim == null || !int.TryParse(accountIdClaim.Value, out int accountId))
            {
                return Unauthorized("Invalid or missing AccountId.");
            }

            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Flower)
                .FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            // Chuyển đổi Cart thành CartDTO
            var cartDTO = new CartDTO
            {
                CartId = cart.CartId,
                AccountId = cart.AccountId,
                CartItems = cart.CartItems.Select(ci => new CartItemDTO
                {
                    CartItemId = ci.CartItemId,
                    FlowerId = ci.FlowerId,
                    FlowerName = ci.Flower.FlowerName, // Chỉ lấy tên hoa
                    Quantity = ci.Quantity,
                    FlowerPrice = ci.Flower.FlowerPrice
                }).ToList()
            };

            return Ok(cartDTO);
        }

        [HttpDelete("remove-cart-item/{cartItemId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim == null || !int.TryParse(accountIdClaim.Value, out int accountId))
            {
                return Unauthorized("Invalid or missing AccountId.");
            }

            var cartItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.Cart.AccountId == accountId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Xóa CartItem
            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Cart item removed successfully." });
        }


        [HttpPost("add-to-cart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            // Lấy AccountId từ token
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim == null || !int.TryParse(accountIdClaim.Value, out int accountId))
            {
                return Unauthorized("Invalid or missing AccountId.");
            }

            // Kiểm tra giỏ hàng của người dùng
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (cart == null)
            {
                // Tạo giỏ hàng mới nếu chưa có
                cart = new Cart { AccountId = accountId };
                _dbContext.Carts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }

            // Kiểm tra sản phẩm
            var flower = await _dbContext.Flowers.FindAsync(request.FlowerId);
            if (flower == null)
            {
                return NotFound("Flower not found.");
            }
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.FlowerId == request.FlowerId);
            int count = 0;
            if (cartItem == null)
                count = request.Quantity;
            else
                count = request.Quantity + cartItem.Quantity;

            // Kiểm tra số lượng tồn kho
            if (flower.FlowerQuantity < count)
            {
                return BadRequest("Insufficient flower stock.");
            }

            // Tìm sản phẩm trong giỏ hàng
            
            if (cartItem != null)
            {
                // Cập nhật số lượng nếu sản phẩm đã tồn tại
                cartItem.Quantity += request.Quantity;
            }
            else
            {
                // Thêm sản phẩm mới
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    FlowerId = request.FlowerId,
                    Quantity = request.Quantity
                };
                cart.CartItems.Add(cartItem);
            }

            // Trừ số lượng tồn kho
           //flower.FlowerQuantity -= request.Quantity;

            // Lưu thay đổi vào database
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Flower added to cart successfully." });
        }

        [HttpPut("update-cart-item/{cartItemId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemRequest request)
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim == null || !int.TryParse(accountIdClaim.Value, out int accountId))
            {
                return Unauthorized("Invalid or missing AccountId.");
            }

            var cartItem = await _dbContext.CartItems
                .Include(ci => ci.Flower)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.Cart.AccountId == accountId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Kiểm tra số lượng hoa trong kho
            if (cartItem.Flower.FlowerQuantity < request.Quantity)
            {
                return BadRequest("Insufficient flower stock.");
            }

            // Cập nhật số lượng
            cartItem.Quantity = request.Quantity;

            // Lưu thay đổi
            _dbContext.CartItems.Update(cartItem);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Cart item updated successfully." });
        }

    }
}
