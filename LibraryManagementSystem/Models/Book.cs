using System;

namespace LibraryManagement.Models
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string Isbn { get; set; } = "";
        public bool IsBorrowed { get; set; } = false;

        public void MarkBorrowed() => IsBorrowed = true;
        public void MarkReturned() => IsBorrowed = false;
    }
}