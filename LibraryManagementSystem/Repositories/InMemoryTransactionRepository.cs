using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly List<TransactionRecord> _transactions = new();

        public void Add(TransactionRecord item) => _transactions.Add(item);

        public void Delete(Guid id) => _transactions.RemoveAll(t => t.Id == id);

        public IEnumerable<TransactionRecord> GetAll() => _transactions;

        public TransactionRecord? GetById(Guid id) => _transactions.FirstOrDefault(t => t.Id == id);

        public IEnumerable<TransactionRecord> GetByMemberId(Guid memberId) => _transactions.Where(t => t.MemberId == memberId);

        public IEnumerable<TransactionRecord> GetActiveTransactions() => _transactions.Where(t => !t.IsReturned);

        public void Update(TransactionRecord item)
        {
            var idx = _transactions.FindIndex(t => t.Id == item.Id);
            if (idx >= 0) _transactions[idx] = item;
        }
    }
}