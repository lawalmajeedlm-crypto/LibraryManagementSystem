using System;
using System.Collections.Generic;

namespace LibraryManagement.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(Guid id);
        void Add(T item);
        void Update(T item);
        void Delete(Guid id);
    }
}