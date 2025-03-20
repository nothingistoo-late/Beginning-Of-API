using Final4.IRepository;
using Final4.IService;
using Final4.Model.Entities;
using Microsoft.IdentityModel.Tokens;
using Repositories.Commons;

namespace Final4.Service
{
    public class FlowerService : IFlowerService
    {
        private readonly IFlowerRepository _flowerRepository;
        public FlowerService(IFlowerRepository flowerRepository)
        {
            _flowerRepository = flowerRepository;
        }

        public async Task<ApiResult<List<Flower>>> GetAllFlowerAsync()
        {
            var flowers = await _flowerRepository.GetAllAsync();
            if (flowers == null)
                return ApiResult<List<Flower>>.Error(null, "nothing found");
            return ApiResult<List<Flower>>.Succeed(flowers, "Get All Flower Compeleted");
            //return ApiResult<List<Flower>>.Succeed(flowers, "success");
        }

        public async Task<ApiResult<List<Flower>>> GetAllFlowerByNameAsync(string flowerName)
        {
            var flowers = await _flowerRepository.GetAllHaveFilterAsync(x => x.FlowerName.ToLower().Contains(flowerName.ToLower()));
            if (flowers.IsNullOrEmpty())
                return ApiResult<List<Flower>>.Error(null, "nothing found");
            return ApiResult<List<Flower>>.Succeed(flowers, "Get All Flower Compeleted");
        }
    }
}
