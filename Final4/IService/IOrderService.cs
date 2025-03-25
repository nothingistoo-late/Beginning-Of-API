using Final4.DTO.Order;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;

namespace Final4.IService
{
    public interface IOrderService
    {
        public Task<ApiResult<List<GetAllOrder>>> GetAllOrderByUserIdAsync(int id);
        public Task<ApiResult<List<Order>>> GetOrderByEmail(string gmail);
        public Task<ApiResult<AddOrder>> AddOrder(AddOrder obj);
        public Task<ApiResult<int>> DeleteOrderByOrderId(int id);
        public Task<ApiResult<bool>> Checkout(CheckoutRequest request);


    }
}
