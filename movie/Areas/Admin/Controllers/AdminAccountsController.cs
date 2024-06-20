using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppBlog.Extension;
using AppBlog.Helpers;
using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Web.Areas.Admin.Models;
using Movie_Web.Models;
using PagedList.Core;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly MoviesContext _context;
        public INotyfService _notifyService { get; }

        public AdminAccountsController(MoviesContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/AdminAccounts
        

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "Page_Size").Value;
            int pageSize = pageSize1 == null ? 1 : pageSize1.Value;
            var lsAccounts = _context.Accounts.Include(a => a.Role).OrderByDescending(x => x.CreateDate);
            PagedList<Account> models = new PagedList<Account>(lsAccounts, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("dang-nhap.html", Name = "Login")]

        //public IActionResult Login(string? returnUrl = null)
        //{
        //    var taikhoanID = HttpContext.Session.GetString("AccountId");
        //    if (taikhoanID != null)
        //        return RedirectToAction("Index", "Home", new { Area = "Admin" });
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}


        //[HttpPost]
        //[AllowAnonymous]
        //[Route("dang-nhap.html", Name = "Login")]
        //public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            Account? kh = _context.Accounts
        //                .Include(a => a.Role)
        //                .SingleOrDefault(a => a.Email!.ToLower() == model.Email!.ToLower().Trim());

        //            if (kh == null)
        //            {
        //                ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
        //                return View(model);
        //            }
        //            string pass;
        //            if (kh.Salt == null)
        //            {
        //                pass = model.Password!.ToMD5().Trim();
        //            }
        //            else
        //            {
        //                //ToMD5()
        //                pass = (model.Password!.ToMD5().Trim() + kh.Salt!.Trim());
        //            }
        //            if (kh.Password!.ToMD5().Trim() != pass)
        //            {
        //                ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
        //                return View(model);
        //            }
        //            //Dang nhap thanh cong

        //            //ghi nhan thoi gian dang nhap
        //            kh.LastLogin = DateTime.Now;
        //            _context.Update(kh);
        //            await _context.SaveChangesAsync();

        //            var taikhoanID = HttpContext.Session.GetString("AccountId");
        //            //Identity
        //            // luu session makh
        //            HttpContext.Session.SetString("AccountId", kh.AccountId.ToString());
        //            //Identity
        //            var userClaims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, kh.FullName!),
        //                new Claim(ClaimTypes.Email, kh.Email!),
        //                new Claim("AccountId", kh.AccountId.ToString()),
        //                new Claim("RoleId", kh.RoleId!.ToString()!),
        //                new Claim(ClaimTypes.Role, kh.Role!.RoleName!)
        //            };
        //            var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
        //            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
        //            await HttpContext.SignInAsync(userPrincipal);

        //            //if (Url.IsLocalUrl(returnUrl))
        //            //{
        //            //    return Redirect(returnUrl);
        //            //}
        //            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        //        }
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
        //    }
        //    return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
        //}



        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/AdminAccounts/Create
        //public IActionResult Create()
        //{
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
        //    return View();
        //}

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("AccountId,Email,Phone,Password,Active,FullName,RoleId,LastLogin,CreateDate")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(account);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
        //    return View(account);
        //}

        // GET: Admin/AdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Email,Phone,Password,Active,FullName,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }

        
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
