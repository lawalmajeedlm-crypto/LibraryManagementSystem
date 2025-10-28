using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        Member? GetByEmail(string email);
    }
}