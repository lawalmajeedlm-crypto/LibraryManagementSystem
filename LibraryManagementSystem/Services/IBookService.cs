using LibraryManagement.Models;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();
        Book? GetById(Guid id);
        void Create(Book book);
        void Update(Book book);
        void Delete(Guid id);
        IEnumerable<Book> GetAvailable();
        IEnumerable<Book> GetBorrowed();
    }
}