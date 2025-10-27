using LibraryManagement.Models;
using LibraryManagement.Services;
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

        public IActionResult Index()
        {
            var model = _books.GetAll();
            return View(model);
        }

        public IActionResult Available()
        {
            return View("Index", _books.GetAvailable());
        }

        public IActionResult Borrowed()
        {
            return View("Index", _books.GetBorrowed());
        }

        // Create is now available to everyone (anonymous or authenticated)
        public IActionResult Create() => View(new Book());

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid) return View(book);
            _books.Create(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            var b = _books.GetById(id);
            if (b == null) return NotFound();
            return View(b);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid) return View(book);
            _books.Update(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            _books.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(Guid id)
        {
            var b = _books.GetById(id);
            if (b == null) return NotFound();
            return View(b);
        }
    }
}