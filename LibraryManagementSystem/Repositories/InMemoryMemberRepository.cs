using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement.Repositories
{
    public class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Member> _members = new();
        private readonly IPasswordHasher<Member> _passwordHasher;

        public InMemoryMemberRepository(IPasswordHasher<Member> passwordHasher)
        {
            _passwordHasher = passwordHasher;

            // Seed sample data with HASHED passwords
            var alice = new Member { FullName = "Alice Johnson", Email = "alice@example.com" };
            alice.Password = _passwordHasher.HashPassword(alice, "password123");

            var bob = new Member { FullName = "Bob Smith", Email = "bob@example.com" };
            bob.Password = _passwordHasher.HashPassword(bob, "password123");

            _members.AddRange(new[] { alice, bob });
        }

        public void Add(Member item) => _members.Add(item);
        public void Delete(Guid id) => _members.RemoveAll(m => m.Id == id);
        public IEnumerable<Member> GetAll() => _members;
        public Member? GetById(Guid id) => _members.FirstOrDefault(m => m.Id == id);

        // NEW: Get member by email for login
        public Member? GetByEmail(string email) => _members.FirstOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public void Update(Member item)
        {
            var idx = _members.FindIndex(m => m.Id == item.Id);
            if (idx >= 0) _members[idx] = item;
        }
    }
}