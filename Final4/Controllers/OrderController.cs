using Final4.Data;
using Final4.DTO.Order;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return Ok(await _dbContext.Orders.ToListAsync());
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<ActionResult> AddOrder(AddOrder obj)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Email == obj.UserEmail);
            if (account != null)  // Kiểm tra nếu account không phải là null
            {
                var orderEntity = new Order
                {
                    OrderName = obj.OrderName,
                    UserId = account.Id  // Lấy UserId từ account
                };

                // Tiến hành lưu Order nếu cần
                await _dbContext.Orders.AddAsync(orderEntity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Xử lý khi không tìm thấy account (ví dụ: trả về lỗi hoặc thông báo)
                throw new Exception("Account không tồn tại.");
            }
            return Ok(obj);
        }
    }
}
