using LibraryManagement.Models;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Services
{
    public interface ITransactionService
    {
        IEnumerable<TransactionRecord> GetAll();
        IEnumerable<TransactionRecord> GetActive();
        IEnumerable<TransactionRecord> GetByMember(Guid memberId);
        TransactionRecord? BorrowBook(Guid bookId, Guid memberId);
        bool ReturnBook(Guid transactionId);
    }
}