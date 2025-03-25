using Microsoft.EntityFrameworkCore.Storage;

namespace Final4.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        public IAccountRepository AccountRepository { get; }
        public IFlowerRepository FlowerRepository { get; }
        public IOrderRepository OrderRepository { get; }
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();


    }
}
