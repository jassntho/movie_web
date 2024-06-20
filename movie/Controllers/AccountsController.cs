using AppBlog.Extension;
using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Movie_Web.Models;
using Movie_Web.ModelsView;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Movie_Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly MoviesContext _context;
        public INotyfService _notifyService { get; }
        public AccountsController(MoviesContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
      

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangKyTaiKhoan()
        {
            
            return View();

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user1 = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email == taikhoan.Email);
                    if (user1 != null)
                    {
                        _notifyService.Error("Email nay da duoc su dung", 2);
                        ViewBag.Error = "Email này đã được sử dụng";
                        return View(taikhoan);
                    }
                   
                    if (taikhoan.Password != taikhoan.ConfirmPassword)
                    {
                        _notifyService.Error("Mat khau xac nhan phai giong nhau", 2);
                        ViewBag.Error = "Mat khau xac nhan phai giong nhau";
                        return View(taikhoan);
                    }


                    Account user = new Account
                    {
                        FullName = taikhoan.FullName,
                        Email = taikhoan.Email.Trim().ToLower(),
                        
                        Password = taikhoan.Password.Trim().ToMD5(),

                        Active = true,
                        CreateDate = DateTime.Now,
                        RoleId = 2
                    };
                    try
                    {
                        user.LastLogin = DateTime.Now;
                        _context.Accounts.Add(user);
                        await _context.SaveChangesAsync();
                        _notifyService.Success("Create User Success", 2);

                        //Luu session
                        HttpContext.Session.SetString("AccountId", user.AccountId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("AccountId");

                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FullName),
                            
                            new Claim("AccountId",user.AccountId.ToString()),
                            new Claim("RoleId", user.RoleId!.ToString()!),
                            

                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }

        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]
        public IActionResult Login(string? returnUrl = null)
        {
            var userID = HttpContext.Session.GetString("AccountId");
            if (userID != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel account, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Accounts.Include(a => a.Role).AsNoTracking().SingleOrDefault(x => x.Email == account.Email);
                    if (user == null)
                    {
                        _notifyService.Error("Thông tin đăng nhập chưa chính xác", 2);
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(account);
                        //return RedirectToAction("DangKyTaiKhoan");
                    }
                    string pass = account.Password.ToMD5();
                    if (user.Password != pass)
                    {
                        _notifyService.Error("Thông tin đăng nhập chưa chính xác", 2);
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(account);
                    }
                    if (user.Active == false)
                    {
                        _notifyService.Error("Tai khoan da bi chan boi admin", 2);
                        return View(account);
                    }
                    user.LastLogin = DateTime.Now;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Dang Nhap thanh cong", 2);

                    HttpContext.Session.SetString("AccountId", user.AccountId.ToString());
                    var userID = HttpContext.Session.GetString("AccountId");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Role, user.Role!.RoleName!),
                        new Claim("AccountId",user.AccountId.ToString()),
                        new Claim("RoleId", user.RoleId!.ToString()!),
                                    
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);

                    if (user.RoleId == 1)
                    {
                        return RedirectToAction("Index", "Home", new { Area = "Admin" });
                    }
                    else
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            catch
            {
                _notifyService.Error("Dang Nhap that bai", 2);
                return RedirectToAction("Index", "Home");
            }
            return View(account);
        }

        [Route("dang-xuat.html", Name = "Logout")]
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
