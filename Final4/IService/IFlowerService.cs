﻿using Final4.DTO.Flower;
using Final4.Model.Entities;
using Repositories.Commons;

namespace Final4.IService
{
    public interface IFlowerService
    {
        Task<ApiResult<List<Flower>>> GetAllFlowerAsync();
        Task<ApiResult<List<Flower>>> GetAllFlowerByNameAsync(string flowerName);
        Task<ApiResult<List<Flower>>> AddFlowerAsync(List<AddFlower> flower);
    }
}
