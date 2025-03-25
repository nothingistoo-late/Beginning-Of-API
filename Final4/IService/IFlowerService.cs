using Final4.DTO.Flower;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Commons;

namespace Final4.IService
{
    public interface IFlowerService
    {
        Task<ApiResult<List<Flower>>> GetAllFlowerAsync();
        Task<ApiResult<List<Flower>>> GetAllFlowerByNameAsync(string flowerName);
        Task<ApiResult<List<Flower>>> AddFlowerAsync(List<AddFlowerDTO> flower);
        Task<ApiResult<UpdateFlowerDTO>> UpdateFlowerAsync(UpdateFlowerDTO flower);

        Task<ApiResult<int>> DeleteFlowerAsync(int id);
        Task<ApiResult<List<Flower>>> SearchFlower(string? name, string? description, decimal? priceFrom, decimal? priceTo, decimal? quantity);
    }


}
