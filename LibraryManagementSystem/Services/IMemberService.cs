using LibraryManagement.Models;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Services
{
    public interface IMemberService
    {
        IEnumerable<Member> GetAll();
        Member? GetById(Guid id);
        void Create(Member member);
        void Update(Member member);
        void Delete(Guid id);
    }
}