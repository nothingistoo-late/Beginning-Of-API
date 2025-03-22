using System.Collections.Generic;
using Final4.Data;
using Final4.DTO.OrderDetail;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final4.Controllers
{
    public class OrderDetailController : Controller
    {
        private ApplicationDBContext _dbContext;

        public OrderDetailController(ApplicationDBContext DBContext)
        {
            _dbContext = DBContext;
        }
        [HttpGet]
        [Route("GetOrderDetailByOrderId/{id}")]
        public async Task<IActionResult> GetOrderDetailByOrderId(int id)
        {
            // Lấy danh sách các OrderDetail theo OrderId
            var orderDetailList = await _dbContext.OrderDetails
                .Where(o => o.OrderId == id)
                .Select(od => new GetOrderDetail
                {
                    OrderId = id,
                    OrderDetailId = od.Id,
                    FlowerId = od.FlowerId,
                    Quantity = od.Quantity,
                    Note = od.Note
                })
                .ToListAsync();

            // Kiểm tra danh sách kết quả
            if (orderDetailList.Any())
            {
                return Ok(orderDetailList);
            }
            else
            {
                return NotFound($"Not Found Any Order Detail With Order ID: {id}");
            }
        }
    }
}
