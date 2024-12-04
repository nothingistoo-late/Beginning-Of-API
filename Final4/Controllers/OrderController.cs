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
    }
}
