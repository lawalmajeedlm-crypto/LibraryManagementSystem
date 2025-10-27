using System;
using System.Collections.Generic;

namespace LibraryManagement.Models
{
    public class Member
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";

        // Plain-text password for demo only (in-memory). Do NOT use in production.
        public string Password { get; set; } = "";

        // Denormalized list for quick lookup in-memory.
        public List<Guid> BorrowedBookIds { get; set; } = new List<Guid>();
    }
}