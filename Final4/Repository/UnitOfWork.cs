using Final4.Data;
using Final4.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Final4.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IFlowerRepository _flowerRepository;
        private readonly IOrderRepository _orderRepository;

        public UnitOfWork(ApplicationDBContext dbContext, IAccountRepository accountRepository, IFlowerRepository flowerRepository, IOrderRepository orderRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _flowerRepository = flowerRepository;
            _orderRepository = orderRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;
        public IFlowerRepository FlowerRepository => _flowerRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

    }
}
