using LibraryManagement.Models;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetAvailableBooks();
        IEnumerable<Book> GetBorrowedBooks();
    }
}