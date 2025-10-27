using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _members;
        private readonly ITransactionService _transactions;

        public MembersController(IMemberService members, ITransactionService transactions)
        {
            _members = members;
            _transactions = transactions;
        }

        public IActionResult Index()
        {
            return View(_members.GetAll());
        }

        public IActionResult Details(Guid id)
        {
            var m = _members.GetById(id);
            if (m == null) return NotFound();
            var tx = _transactions.GetByMember(id);
            ViewBag.Transactions = tx;
            return View(m);
        }

        public IActionResult Create() => View(new Member());

        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        {
            if (!ModelState.IsValid) return View(member);

            // Basic server-side validation for password (demo)
            if (string.IsNullOrWhiteSpace(member.Password))
            {
                ModelState.AddModelError(nameof(member.Password), "Password is required.");
                return View(member);
            }

            _members.Create(member);

            // Auto sign-in after registration (convenience for demo)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, member.FullName),
                new Claim("MemberId", member.Id.ToString()),
                new Claim(ClaimTypes.Email, member.Email),
                new Claim(ClaimTypes.Role, "Member")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            var m = _members.GetById(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost]
        public IActionResult Edit(Member member)
        {
            if (!ModelState.IsValid) return View(member);
            _members.Update(member);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            _members.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}