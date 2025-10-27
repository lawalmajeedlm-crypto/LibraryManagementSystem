using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement.Repositories
{
    public class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Member> _members = new();

        public InMemoryMemberRepository()
        {
            // NOTE: passwords are plain-text for this demo (in-memory). Use hashed passwords in real apps.
            _members.AddRange(new[]
            {
                new Member { FullName = "Alice Johnson", Email = "alice@example.com", Password = "password123" },
                new Member { FullName = "Bob Smith", Email = "bob@example.com", Password = "password123" }
            });
        }

        public void Add(Member item) => _members.Add(item);

        public void Delete(Guid id) => _members.RemoveAll(m => m.Id == id);

        public IEnumerable<Member> GetAll() => _members;

        public Member? GetById(Guid id) => _members.FirstOrDefault(m => m.Id == id);

        public Member? GetByEmail(string email) => _members.FirstOrDefault(m => string.Equals(m.Email, email, StringComparison.OrdinalIgnoreCase));

        public void Update(Member item)
        {
            var idx = _members.FindIndex(m => m.Id == item.Id);
            if (idx >= 0) _members[idx] = item;
        }
    }

    // Extend the interface locally so we can lookup by email easily (in-memory only).
    public static class InMemoryMemberRepositoryExtensions
    {
        public static Member? GetByEmail(this IMemberRepository repo, string email)
        {
            if (repo is InMemoryMemberRepository r) return r.GetByEmail(email);
            // Fallback: enumerate
            return repo.GetAll().FirstOrDefault(m => string.Equals(m.Email, email, StringComparison.OrdinalIgnoreCase));
        }
    }
}