using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System;
using System.Collections.Generic; // REQUIRED for IEnumerable<T>

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _books;

        public BookService(IBookRepository books)
        {
            _books = books;
        }

        public void Create(Book book) => _books.Add(book);
        public void Delete(Guid id) => _books.Delete(id);
        public IEnumerable<Book> GetAll() => _books.GetAll();
        public IEnumerable<Book> GetAvailable() => _books.GetAvailableBooks();
        public Book? GetById(Guid id) => _books.GetById(id);
        public IEnumerable<Book> GetBorrowed() => _books.GetBorrowedBooks();
        public void Update(Book book) => _books.Update(book);
    }
}