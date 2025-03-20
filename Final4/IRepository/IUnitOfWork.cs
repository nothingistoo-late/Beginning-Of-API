﻿using Microsoft.EntityFrameworkCore.Storage;

namespace Final4.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        public IAccountRepository AccountRepository { get; }
        Task<int> SaveChangeAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();


    }
}
