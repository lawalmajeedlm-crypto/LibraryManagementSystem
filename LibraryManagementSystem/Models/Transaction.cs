using System;

namespace LibraryManagement.Models
{
    public class TransactionRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BookId { get; set; }
        public Guid MemberId { get; set; }
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; } = null;

        public bool IsReturned => ReturnedAt.HasValue;

        public void MarkReturned()
        {
            ReturnedAt = DateTime.UtcNow;
        }
    }
}