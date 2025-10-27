using LibraryManagement.Models;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Repositories
{
    public interface ITransactionRepository : IRepository<TransactionRecord>
    {
        IEnumerable<TransactionRecord> GetByMemberId(Guid memberId);
        IEnumerable<TransactionRecord> GetActiveTransactions();
    }
}