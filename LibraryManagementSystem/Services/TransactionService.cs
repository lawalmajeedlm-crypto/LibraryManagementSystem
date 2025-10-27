using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactions;
        private readonly IBookRepository _books;
        private readonly IMemberRepository _members;

        public TransactionService(ITransactionRepository transactions, IBookRepository books, IMemberRepository members)
        {
            _transactions = transactions;
            _books = books;
            _members = members;
        }

        public TransactionRecord? BorrowBook(Guid bookId, Guid memberId)
        {
            var book = _books.GetById(bookId);
            if (book == null || book.IsBorrowed) return null;

            var member = _members.GetById(memberId);
            if (member == null) return null;

            // mark book borrowed
            book.MarkBorrowed();
            _books.Update(book);

            // update member borrowed list (simple denormalized)
            member.BorrowedBookIds.Add(book.Id);
            _members.Update(member);

            var tx = new TransactionRecord
            {
                BookId = book.Id,
                MemberId = member.Id,
                BorrowedAt = DateTime.UtcNow
            };

            _transactions.Add(tx);
            return tx;
        }

        public bool ReturnBook(Guid transactionId)
        {
            var tx = _transactions.GetById(transactionId);
            if (tx == null || tx.IsReturned) return false;

            var book = _books.GetById(tx.BookId);
            var member = _members.GetById(tx.MemberId);

            tx.MarkReturned();
            _transactions.Update(tx);

            if (book != null)
            {
                book.MarkReturned();
                _books.Update(book);
            }

            if (member != null)
            {
                member.BorrowedBookIds.Remove(tx.BookId);
                _members.Update(member);
            }

            return true;
        }

        public IEnumerable<TransactionRecord> GetAll() => _transactions.GetAll();

        public IEnumerable<TransactionRecord> GetActive() => _transactions.GetActiveTransactions();

        public IEnumerable<TransactionRecord> GetByMember(Guid memberId) => _transactions.GetByMemberId(memberId);
    }
}