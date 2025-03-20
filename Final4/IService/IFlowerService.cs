using Final4.Model.Entities;
using Repositories.Commons;

namespace Final4.IService
{
    public interface IFlowerService
    {
        Task<ApiResult<Flower>> GetAllFlowerAsync();
        Task<ApiResult<List<Flower>>> GetAllFlowerByNameAsync();
    }
}
