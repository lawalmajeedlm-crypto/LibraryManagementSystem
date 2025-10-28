using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize] // Transactions require login
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

        public IActionResult Index() => View(_transactions.GetAll());
        public IActionResult Active() => View("Index", _transactions.GetActive());

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
    }
}