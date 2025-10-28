using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _members;
        private readonly ITransactionService _transactions;
        private readonly IPasswordHasher<Member> _passwordHasher;

        public MembersController(IMemberService members, ITransactionService transactions, IPasswordHasher<Member> passwordHasher)
        {
            _members = members;
            _transactions = transactions;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index() => View(_members.GetAll());

        public IActionResult Details(Guid id)
        {
            var m = _members.GetById(id);
            if (m == null) return NotFound();
            var tx = _transactions.GetByMember(id);
            ViewBag.Transactions = tx;
            return View(m);
        }

        public IActionResult Create() => View(new Member()); // This is the Registration page

        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(member.Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
                return View(member);
            }

            // Hash the password before saving
            member.Password = _passwordHasher.HashPassword(member, member.Password);

            _members.Create(member);

            // Auto sign-in after registration
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Name, member.FullName),
                new Claim(ClaimTypes.Email, member.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction(nameof(Index));
        }

        // Edit, Delete actions require authorization but we keep them open for librarians (simplicity)
        public IActionResult Edit(Guid id)
        {
            var m = _members.GetById(id);
            if (m == null) return NotFound();
            m.Password = string.Empty; // Don't expose hash
            return View(m);
        }

        [HttpPost]
        public IActionResult Edit(Member member)
        {
            if (!ModelState.IsValid) return View(member);

            var existingMember = _members.GetById(member.Id);
            if (existingMember == null) return NotFound();

            existingMember.FullName = member.FullName;
            existingMember.Email = member.Email;

            // Only update the password hash if a new password was entered
            if (!string.IsNullOrEmpty(member.Password))
            {
                existingMember.Password = _passwordHasher.HashPassword(existingMember, member.Password);
            }

            _members.Update(existingMember);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            _members.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}