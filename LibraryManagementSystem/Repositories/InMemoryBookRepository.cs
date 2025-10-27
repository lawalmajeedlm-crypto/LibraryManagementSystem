using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();

        public InMemoryBookRepository()
        {
            _books.AddRange(new[]
            {
                new Book { Title = "Clean Code", Author = "Robert C. Martin", Isbn = "9780132350884" },
                new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Isbn = "9780201616224" },
                new Book { Title = "Design Patterns", Author = "GoF", Isbn = "9780201633610" }
            });
        }

        public void Add(Book item) => _books.Add(item);

        public void Delete(Guid id) => _books.RemoveAll(b => b.Id == id);

        public IEnumerable<Book> GetAll() => _books;

        public Book? GetById(Guid id) => _books.FirstOrDefault(b => b.Id == id);

        public IEnumerable<Book> GetAvailableBooks() => _books.Where(b => !b.IsBorrowed);

        public IEnumerable<Book> GetBorrowedBooks() => _books.Where(b => b.IsBorrowed);

        public void Update(Book item)
        {
            var idx = _books.FindIndex(b => b.Id == item.Id);
            if (idx >= 0) _books[idx] = item;
        }
    }
}