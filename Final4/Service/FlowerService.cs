using AutoMapper;
using Final4.DTO.Flower;
using Final4.IRepository;
using Final4.IService;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ApiResult<List<Flower>>> AddFlowerAsync(List<AddFlowerDTO> flowers)
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

        public async Task<ApiResult<int>> DeleteFlowerAsync(int id)
        {
            var flowerEntity = await _unitOfWork.FlowerRepository.GetByIdAsync(id);
            if (flowerEntity == null)
                return ApiResult<int>.Error(default, "Flower Id = " + id + " Does Not Exist!!\n Please Check FlowerID Again!!");
            await _unitOfWork.FlowerRepository.Delete(flowerEntity);
            await _unitOfWork.SaveChangesAsync();
            return ApiResult<int>.Succeed(id, "Delete Flower With " + id + " Successfully!!");
        }

        public async Task<ApiResult<List<Flower>>> GetAllFlowerAsync()
        {
            var flowers = await _unitOfWork.FlowerRepository.GetAllAsync();
            if (flowers == null)
                return ApiResult<List<Flower>>.Error(null, "Nothing found!!!");
            return ApiResult<List<Flower>>.Succeed(flowers, "Get All Flower Compeleted");
        }

        public async Task<ApiResult<List<Flower>>> GetAllFlowerByNameAsync(string flowerName)
        {
            var flowers = await _unitOfWork.FlowerRepository.GetAllHaveFilterAsync(x => x.FlowerName.ToLower().Contains(flowerName.ToLower()));
            if (flowers.IsNullOrEmpty())
                return ApiResult<List<Flower>>.Error(null, "Have No Flower With Name: " + flowerName);
            return ApiResult<List<Flower>>.Succeed(flowers, "Get All Flower Compeleted");
        }

        public async Task<ApiResult<List<Flower>>> SearchFlower(string? name, string? description, decimal? priceFrom, decimal? priceTo, decimal? quantity)
        {

            var listFlower = await _unitOfWork.FlowerRepository.GetAllAsync();
            // Lọc theo tên sản phẩm
            if (!string.IsNullOrEmpty(name))
            {
                listFlower = listFlower.Where(p => p.FlowerName.ToLower().Contains(name.ToLower())).ToList();
            }

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(description))
            {
                listFlower = listFlower.Where(p => p.FlowerDescription.ToLower().Contains(description.ToLower())).ToList();
            }

            // Lọc theo giá thấp nhất (>=)
            if (priceFrom.HasValue)
            {
                listFlower = listFlower.Where(p => p.FlowerPrice >= priceFrom.Value).ToList();
            }

            // Lọc theo giá cao nhất (<=)
            if (priceTo.HasValue)
            {
                listFlower = listFlower.Where(p => p.FlowerPrice <= priceTo.Value).ToList();
            }

            if (listFlower.IsNullOrEmpty())
                return ApiResult<List<Flower>>.Error(null, "No Flower With That Requirement!!");
            return ApiResult<List<Flower>>.Succeed(listFlower, "Search Flower Successfully");
        }


        public async Task<ApiResult<UpdateFlowerDTO>> UpdateFlowerAsync(UpdateFlowerDTO flower)
        {

            var flowerEntity = await _unitOfWork.FlowerRepository.GetByIdAsync(flower.FlowerId);
            if (flowerEntity == null)
                return ApiResult<UpdateFlowerDTO>.Error(flower, "Flower Id = " + flower.FlowerId + " Does Not Exist!!\n Please Check FlowerID Again!!");
            else

            {
                // Chỉ cập nhật các trường nếu có giá trị
                if (!string.IsNullOrEmpty(flower.FlowerName))
                    flowerEntity.FlowerName = flower.FlowerName;

                if (!string.IsNullOrEmpty(flower.FlowerDescription))
                    flowerEntity.FlowerDescription = flower.FlowerDescription;

                if (!string.IsNullOrEmpty(flower.ImgUrl))
                    flowerEntity.FlowerImgUrl = flower.ImgUrl;

                if (flower.FlowerQuantity.HasValue)
                    flowerEntity.FlowerQuantity = flower.FlowerQuantity.Value;

                if (flower.FlowerPrice.HasValue)
                    flowerEntity.FlowerPrice = flower.FlowerPrice.Value;
                //_mapper.Map(flower,flowerEntity);

                await _unitOfWork.SaveChangesAsync();
                return ApiResult<UpdateFlowerDTO>.Succeed(flower, "Update Flower Successfully");
            }
        }

        

        //public async Task<ApiResult>
    }
}
