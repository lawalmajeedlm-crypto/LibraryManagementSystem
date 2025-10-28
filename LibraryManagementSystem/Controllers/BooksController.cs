using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryManagement.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _books;
        public BooksController(IBookService books)
        {
            _books = books;
        }

        public IActionResult Index() => View(_books.GetAll());
        public IActionResult Available() => View("Index", _books.GetAvailable());
        public IActionResult Borrowed() => View("Index", _books.GetBorrowed());
        public IActionResult Details(Guid id)
        {
            var b = _books.GetById(id);
            return b == null ? NotFound() : View(b);
        }

        [Authorize] // Only logged-in members can add books
        public IActionResult Create() => View(new Book());

        [Authorize]
        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid) return View(book);
            _books.Create(book);
            return RedirectToAction(nameof(Index));
        }

        [Authorize] // Requires login to edit/delete
        public IActionResult Edit(Guid id)
        {
            var b = _books.GetById(id);
            return b == null ? NotFound() : View(b);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid) return View(book);
            _books.Update(book);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Delete(Guid id)
        {
            _books.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}