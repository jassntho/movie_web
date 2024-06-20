using AppBlog.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Web.Data;
using Movie_Web.Models;
using PagedList.Core;
using System.Diagnostics;
using System.Security.Claims;

namespace Movie_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MoviesContext _context;
        private readonly INotyfService _notifyService;
        public HomeController(ILogger<HomeController> logger, MoviesContext context, INotyfService notyfService)
        {
            _logger = logger;
            _context = context;
            _notifyService = notyfService;
        }

        public IActionResult Index(int? page)
        {
            

            int currentMonth = DateTime.Now.Month;
            if (currentMonth == 1)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_1");
                bien.Value++;
            }
            else if (currentMonth == 2)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_2");
                bien.Value++;
            }
            else if (currentMonth == 3)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_3");
                bien.Value++;
            }
            else if (currentMonth == 4)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_4");
                bien.Value++;
            }
            else if (currentMonth == 5)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_5");
                bien.Value++;
            }
            else if (currentMonth == 6)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_6");
                bien.Value++;
            }
            else if (currentMonth == 7)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_7");
                bien.Value++;
            }
            else if (currentMonth == 8)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_8");
                bien.Value++;
            }
            else if (currentMonth == 9)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_9");
                bien.Value++;
            }
            else if (currentMonth == 10)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_10");
                bien.Value++;
            }
            else if (currentMonth == 11)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_11");
                bien.Value++;
            }
            else if (currentMonth == 12)
            {
                var bien = _context.Parameters.FirstOrDefault(x => x.ParameterName == "SoLuotXem_12");
                bien.Value++;
            }

          

            var taikhoanID = HttpContext.Session.GetString("AccountId");
            
            if(taikhoanID != null)
            {
                var fullNameClaim = User.FindFirst(ClaimTypes.Name);
                string fullName = fullNameClaim != null ? fullNameClaim.Value : "";
                ViewBag.FullName = fullName;
            }
            else
            {
                ViewBag.FullName = null;
            }

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 15;
            List<Movie> listMovieHot = _context.Movies.Include(a => a.Categories).Where(a => a.Episode == 1 && a.Status == true).AsNoTracking().ToList();
           
            List<Country> listCountry = _context.Countries.Take(9).AsNoTracking().ToList();
            List<Category> listCategory = _context.Categories.Take(15).AsNoTracking().ToList();

            
            List<Movie> listMovie = _context.Movies.Include(a => a.Categories).Where(a => a.Episode == 1).AsNoTracking().ToList();
            int CountMovie = _context.Movies.Where(a => a.Episode == 1).AsNoTracking().Count();
            PagedList<Movie> models = new PagedList<Movie>(listMovie.AsQueryable(), pageNumber, pageSize);

            int SoTrangMax = (int)Math.Ceiling((double)CountMovie / pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.MovieHot = listMovieHot;
            ViewBag.Cat = listCategory;
            ViewBag.Country = listCountry;
            ViewBag.SoTrangMax = SoTrangMax;

            _context.SaveChangesAsync();

            return View(models);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
