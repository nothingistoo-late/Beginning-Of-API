using AutoMapper;
using Final4.DTO.Order;
using Final4.IRepository;
using Final4.IService;
using Final4.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Repositories.Commons;

namespace Final4.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<ApiResult<AddOrder>> AddOrder(AddOrder obj)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> Checkout(CheckoutRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<int>> DeleteOrderByOrderId(int id)
        {
            throw new NotImplementedException();
        }

        //public async Task<ApiResult<List<Order>>> GetAllOrderAsync()
        //{

        //    var listOrder = await _unitOfWork.OrderRepository.GetQueryable()
        //          .Include(o => o.Account)
        //          .Select(o => new GetAllOrder
        //          {
        //              OrderId = o.Id,
        //              OrderName = o.OrderName,
        //              AccountId = o.AccountId,
        //              AccountName = o.Account.AccountName,
        //              AccountGmail = o.Account!.AccountEmail
        //          })
        //          .ToListAsync();

        //    return ApiResult<List<Order>>.Succeed(listOrder, "Get All Order Compeleted");
        //}

        public async Task<ApiResult<List<GetAllOrder>>> GetAllOrderByUserIdAsync(int id)
        {
            var user = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            if (user == null)
                return ApiResult<List<GetAllOrder>>.Error(null, "User Not Found");

            var listOrder = await _unitOfWork.OrderRepository.GetQueryable()
                .Where(o => o.AccountId == id)
                .Select(o => new GetAllOrder  // ✅ Dùng DTO thay vì entity Order
                {
                    OrderId = o.Id,
                    OrderName = o.OrderName,
                    AccountId = id
                })
                .ToListAsync();

            if (listOrder.IsNullOrEmpty())
                return ApiResult<List<GetAllOrder>>.Error(null, "No Order Found");

            return ApiResult<List<GetAllOrder>>.Succeed(listOrder, "Get All Order Completed");
        }

        public Task<ApiResult<List<Order>>> GetOrderByEmail(string gmail)
        {
            throw new NotImplementedException();
        }
    }
}
