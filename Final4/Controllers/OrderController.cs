using Final4.Data;
using Final4.DTO.Order;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Final4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IConfiguration _configuration;
        private ApplicationDBContext _dbContext;
        private EmailService _emailService;

        public OrderController(ApplicationDBContext DBContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = DBContext;
            _configuration = configuration;
            _emailService = emailService;
        }
        [HttpGet]
        [Route("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder()
        {
            return Ok(await _dbContext.Orders
                                 .Include(o => o.Account)
                                 .Select(o => new GetAllOrder
                                 {
                                     OrderId = o.OrderId,
                                     OrderName = o.OrderName,
                                     AccountId = o.AccountId,
                                     AccountName = o.Account.AccountName,
                                     AccountGmail = o.Account.AccountEmail
                                 })
                                 .ToListAsync());
            // return Ok(await _dbContext.Orders.Include(o=>o.Account.AccountEmail).ToListAsync());
        }
        [HttpGet]
        [Route("GetOrderByGmail{Gmail}")]
        public async Task<IActionResult> GetOrderByEmail(string Gmail)
        {
            var orders = await _dbContext.Orders
                              .Where(o => o.Account != null && o.Account.AccountEmail.ToLower() == Gmail.ToLower()) // Kiểm tra trước khi truy cập
                              .Include(o => o.Account) // Bao gồm dữ liệu Account liên quan
                              .Select(o => new GetOrderByGmail
                              {
                                  OrderId = o.OrderId,
                                  OrderName = o.OrderName,
                                  AccountGmail = o.Account.AccountEmail
                              })
                              .ToListAsync();
            if (orders.IsNullOrEmpty())
                return NotFound();
            return Ok(orders);
        }
        [HttpPost]
        [Route("AddOrder")]
        public async Task<ActionResult> AddOrder(AddOrder obj)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountEmail.ToLower() == obj.UserEmail.ToLower());
            if (account != null)  // Kiểm tra nếu account không phải là null
            {
                var orderEntity = new Order()
                {
                    Account = account,
                    OrderName = obj.OrderName,  
                    OrderStatus = obj.OrderStatus
                };
                await _dbContext.Orders.AddAsync(orderEntity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Xử lý khi không tìm thấy account (ví dụ: trả về lỗi hoặc thông báo)
                return NotFound("Account không tồn tại.");
            }
            return Ok(obj);
        }

        [HttpDelete]
        [Route("DeleteOrderByOrderId{id}")]
        public async Task<IActionResult> DeleteORderByOrderId(int id)
        {
            // Lấy thông tin Order từ cơ sở dữ liệu

            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
            if (order== null) 
                return NotFound();

            // Lấy AccountId của người dùng hiện tại từ thông tin xác thực
            var userAccountId = User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;

            if (userAccountId == null || userAccountId != order.AccountId.ToString())
                return Unauthorized("You are not authorized to delete this order");

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            // Lấy AccountId từ token (JWT)
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
            if (accountIdClaim == null)
            {
                return Unauthorized("AccountId is missing from the token.");
            }
            int accountId = int.Parse(accountIdClaim);

            // Kiểm tra giỏ hàng có tồn tại không
            var cart = await _dbContext.Carts
                                       .Include(c => c.CartItems)
                                       .ThenInclude(ci => ci.Flower)
                                       .FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("Your cart is empty.");
            }

            // Tạo đơn hàng mới
            var order = new Order()
            {
                AccountId = accountId,
                OrderName = request.OrderName ?? "Order from Cart",  // Tên đơn hàng (có thể từ request)
                OrderStatus = "Pending",  // Trạng thái ban đầu là Pending
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var cartItem in cart.CartItems)
            {
                // Kiểm tra số lượng hoa trong kho
                if (cartItem.Quantity > cartItem.Flower.FlowerQuantity)
                {
                    return BadRequest($"Not enough stock for {cartItem.Flower.FlowerName}");
                }

                // Tạo chi tiết đơn hàng
                var orderDetail = new OrderDetail
                {
                    FlowerId = cartItem.FlowerId,
                    Quantity = cartItem.Quantity,
                    Note = cartItem.Flower.FlowerDescription  // Có thể lưu mô tả hoa (nếu cần)
                };

                // Thêm chi tiết đơn hàng vào đơn hàng
                order.OrderDetails.Add(orderDetail);

                // Cập nhật số lượng hoa trong kho
                cartItem.Flower.FlowerQuantity -= cartItem.Quantity;
            }


            // Thêm đơn hàng vào cơ sở dữ liệu
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            string body = _emailService.GenerateOrderEmailBody(order);
            var listUser = await _dbContext.Accounts.ToListAsync();
            Account account = listUser.FirstOrDefault(o => o.AccountId == accountId);
            await _emailService.SendEmailAsync(account.AccountEmail, "Purche Successfully", body);

            // Xóa các sản phẩm trong giỏ hàng sau khi tạo đơn hàng
            _dbContext.CartItems.RemoveRange(cart.CartItems);
            await _dbContext.SaveChangesAsync();

            // Lấy tất cả các bản ghi trong bảng Carts và xóa chúng
            _dbContext.Carts.RemoveRange(_dbContext.Carts.ToList());
            await _dbContext.SaveChangesAsync();


            return Ok(order);
        }


    }
}
