using Final4.Data;
using Microsoft.EntityFrameworkCore;

namespace Final4.Repository
{
    public class UnitOfWork
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IAccountRepository _accountRepository;

        public UnitOfWork(ApplicationDBContext dbContext, IAccountRepository accountRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

    }
}
