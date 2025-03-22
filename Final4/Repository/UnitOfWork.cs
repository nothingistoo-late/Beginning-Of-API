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

        public UnitOfWork(ApplicationDBContext dbContext, IAccountRepository accountRepository, IFlowerRepository flowerRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _flowerRepository = flowerRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;
        public IFlowerRepository FlowerRepository => _flowerRepository;
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
