using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Member
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string FullName { get; set; } = "";
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        // Password stores the hash in the repository, and the plain text for form submission.
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        public List<Guid> BorrowedBookIds { get; set; } = new List<Guid>();
    }
}