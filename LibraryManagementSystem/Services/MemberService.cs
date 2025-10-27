using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _members;

        public MemberService(IMemberRepository members)
        {
            _members = members;
        }

        public void Create(Member member) => _members.Add(member);

        public void Delete(Guid id) => _members.Delete(id);

        public IEnumerable<Member> GetAll() => _members.GetAll();

        public Member? GetById(Guid id) => _members.GetById(id);

        public void Update(Member member) => _members.Update(member);
    }
}