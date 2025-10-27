using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactions;
        private readonly IBookService _books;
        private readonly IMemberService _members;

        public TransactionsController(ITransactionService transactions, IBookService books, IMemberService members)
        {
            _transactions = transactions;
            _books = books;
            _members = members;
        }

        public IActionResult Index()
        {
            var tx = _transactions.GetAll();
            var vm = BuildVm(tx);
            return View(vm);
        }

        public IActionResult Active()
        {
            var tx = _transactions.GetActive();
            var vm = BuildVm(tx);
            return View("Index", vm);
        }

        public IActionResult Borrow()
        {
            ViewBag.Books = _books.GetAvailable();
            ViewBag.Members = _members.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Borrow(Guid bookId, Guid memberId)
        {
            var tx = _transactions.BorrowBook(bookId, memberId);
            if (tx == null)
            {
                TempData["Error"] = "Cannot borrow selected book (maybe already borrowed or data invalid).";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Return(Guid transactionId)
        {
            var ok = _transactions.ReturnBook(transactionId);
            if (!ok) TempData["Error"] = "Return failed (maybe already returned).";
            return RedirectToAction(nameof(Index));
        }

        // helper to map transaction records to view models with related data
        private IEnumerable<TransactionViewModel> BuildVm(IEnumerable<TransactionRecord> txs)
        {
            var books = _books.GetAll().ToDictionary(b => b.Id, b => b);
            var members = _members.GetAll().ToDictionary(m => m.Id, m => m);

            return txs.Select(t => new TransactionViewModel
            {
                Id = t.Id,
                BookId = t.BookId,
                BookTitle = books.TryGetValue(t.BookId, out var b) ? b.Title : "(deleted)",
                MemberId = t.MemberId,
                MemberName = members.TryGetValue(t.MemberId, out var m) ? m.FullName : "(deleted)",
                BorrowedAt = t.BorrowedAt,
                ReturnedAt = t.ReturnedAt
            }).OrderByDescending(v => v.BorrowedAt);
        }
    }
}