using AutoMapper;
using Final4.DTO.Flower;
using Final4.IRepository;
using Final4.IService;
using Final4.Model.Entities;
using Microsoft.IdentityModel.Tokens;
using Repositories.Commons;

namespace Final4.Service
{
    public class FlowerService : IFlowerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FlowerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResult<List<Flower>>> AddFlowerAsync(List<AddFlower> flowers)
        {
           
            try
            {
                var flowerEntities = _mapper.Map<List<Flower>>(flowers);

                // 🔥 Thêm vào database
                await _unitOfWork.FlowerRepository.AddRangeAsync(flowerEntities);

                // 🔥 Lưu thay đổi
                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult == 0)
                    return ApiResult<List<Flower>>.Error(null, "Failed to add flowers");

                return ApiResult<List<Flower>>.Succeed(flowerEntities, "Flowers added successfully");
            }
            catch (Exception ex)
            {
                return ApiResult<List<Flower>>.Error(null, $"An error occurred: {ex.Message}");
            }
        }


        public async Task<ApiResult<List<Flower>>> GetAllFlowerAsync()
        {
            var flowers = await _unitOfWork.FlowerRepository.GetAllAsync();
            if (flowers == null)
                return ApiResult<List<Flower>>.Error(null, "nothing found");
            return ApiResult<List<Flower>>.Succeed(flowers, "Get All Flower Compeleted");
            //return ApiResult<List<Flower>>.Succeed(flowers, "success");
        }

        public async Task<ApiResult<List<Flower>>> GetAllFlowerByNameAsync(string flowerName)
        {
            var flowers = await _unitOfWork.FlowerRepository.GetAllHaveFilterAsync(x => x.FlowerName.ToLower().Contains(flowerName.ToLower()));
            if (flowers.IsNullOrEmpty())
                return ApiResult<List<Flower>>.Error(null, "Have No Flower With Name: " + flowerName);
            return ApiResult<List<Flower>>.Succeed(flowers, "Get All Flower Compeleted");
        }
    }
}
