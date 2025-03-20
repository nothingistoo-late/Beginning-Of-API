using Final4.IService;
using Final4.Model.Entities;
using Repositories.Commons;

namespace Final4.Service
{
    public class FlowerService : IFlowerService
    {
        public Task<ApiResult<Flower>> GetAllFlowerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<Flower>>> GetAllFlowerByNameAsync()
        {
            throw new NotImplementedException();
        }
    }
}
