using System;

namespace LibraryManagement.Models
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = "";
        public Guid MemberId { get; set; }
        public string MemberName { get; set; } = "";
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsReturned => ReturnedAt.HasValue;
    }
}