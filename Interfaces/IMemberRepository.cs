public interface IMemberRepository
{
    Member GetById(int memberId);
    IEnumerable<Member> GetAll();
    void Update(Member member);
}