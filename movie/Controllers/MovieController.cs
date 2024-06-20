using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Web.Data;
using Movie_Web.Models;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Movie_Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly MoviesContext _context;
        private readonly INotyfService _notifyService;
        public MovieController(MoviesContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        [HttpPost]
        public IActionResult SubmitRating([FromBody] RatingModel ratingModel)
        {
            if (ModelState.IsValid)
            {
                var taikhoanID = HttpContext.Session.GetString("AccountId");
                if(taikhoanID == null)
                {                   
                    return Json(new { success = false, message = "Bạn cần đăng nhập để đánh giá!"});
                }
                else
                {
                    var AccountId = User.FindFirst("AccountId");
                    if (AccountId != null)
                    {
                        int AccountID = int.Parse(AccountId.Value);
                        int movieID = ratingModel.MovieId;

                        var RateExits = _context.Rates.FirstOrDefault(x => x.AccountId == AccountID && x.MovieId == movieID);

                        if(RateExits != null)
                        {
                            return Json(new { success = true, message = "Cảm ơn nhé, Bạn đã đánh giá phim này rồi!"});
                        }
                        
                        int rating = ratingModel.Rating;

                        var rate = new Rate();
                        rate.AccountId = AccountID;
                        rate.MovieId = movieID;
                        rate.Rate1 = rating;

                        _context.Rates.Add(rate);
                        _context.SaveChanges();
                        return Json(new { success = true, message = "Đánh giá phim thành công!"});
                    }

                }
                // Xử lý dữ liệu, ví dụ: lưu vào cơ sở dữ liệu
                return Json(new { success = false, message = "Có lỗi xảy ra!"});
            }
            else
            {
                return Json(new { success = false, message = "Có lỗi xảy ra!"});
            }
        }

        [Route("demophim/{Alias}")]
        public IActionResult DemoPhim(string Alias)
        {

            


            var taikhoanID = HttpContext.Session.GetString("AccountId");

            if (taikhoanID != null)
            {
                var fullNameClaim = User.FindFirst(ClaimTypes.Name);
                string fullName = fullNameClaim != null ? fullNameClaim.Value : "";
                ViewBag.FullName = fullName;
            }
            else
            {
                ViewBag.FullName = null;
            }

            if (string.IsNullOrEmpty(Alias))
            {
                return RedirectToAction("Index", "Home");
            }
            
            var models = _context.Movies.Include(a => a.Country).Include(x => x.Type).Include(y => y.Categories).Include(z => z.Actors).Include(x => x.Rates).FirstOrDefault(x => x.Alias == Alias);

            
            

            if (models == null)
            {
                return NotFound();
            }
            else
            {
                if(models.Viewed == null)
                {
                    models.Viewed = 0;
                }
                else
                {
                    models.Viewed += 1;
                }

                string alias_phim = Alias;
                string[] parts = alias_phim.Split('-');
                string lastPart = parts[parts.Length - 1];

                int lastPartIndex = alias_phim.LastIndexOf("-" + lastPart);
                string aliasTenPhim = alias_phim.Substring(0, lastPartIndex + 1);

                //string aliasTenPhim = alias_phim.Replace(lastPart, "");

                
               
                
                int Count = (_context.Movies.Where(x => x.Alias.StartsWith(aliasTenPhim))).Count();
                

                double? Rating = _context.Rates.Where(x => x.MovieId == models.MovieId).Average(x => x.Rate1);

                if(Rating.HasValue)
                {
                    double Rating1 = Math.Round(Rating.Value, 1);
                    ViewBag.Rating = Rating1;
                }
                else
                {
                    ViewBag.Rating = Rating;
                }
                
               

               
                ViewBag.CountPhim = Count;
                ViewBag.AliasTenPhim = aliasTenPhim;
                ViewBag.UrlDemoPhim = Alias;
            }

            List<Movie> listMovieHot = _context.Movies.Include(a => a.Categories).Where(a => a.Episode == 1 && a.Status == true).AsNoTracking().ToList();
            ViewBag.MovieHot = listMovieHot;

            List<Country> listCountry = _context.Countries.Take(9).AsNoTracking().ToList();
            List<Category> listCategory = _context.Categories.Take(15).AsNoTracking().ToList();
            ViewBag.Cat = listCategory;
            ViewBag.Country = listCountry;

            _context.SaveChangesAsync();

            return View(models);
        }

        [Route("xemphim/{Alias}")]
        public IActionResult XemPhim(string Alias)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");

            if (taikhoanID != null)
            {
                var fullNameClaim = User.FindFirst(ClaimTypes.Name);
                string fullName = fullNameClaim != null ? fullNameClaim.Value : "";
                ViewBag.FullName = fullName;
            }
            else
            {
                ViewBag.FullName = null;
            }

            if (string.IsNullOrEmpty(Alias))
            {
                return RedirectToAction("Index", "Home");
            }
            var model = _context.Movies.Include(a => a.Type).FirstOrDefault(x => x.Alias == Alias);
            if (model == null)
            {
                return NotFound();
            }

            string alias_phim = Alias;
            string[] parts = alias_phim.Split('-');
            string lastPart = parts[parts.Length - 1];

            string aliasTenPhim = alias_phim.Replace(lastPart, "");

      

            int Count = _context.Movies.Where(x => x.Alias.StartsWith(aliasTenPhim)).Count();

            string aliasPhimTap1 = aliasTenPhim + "1";

            ViewBag.CountPhim = Count;
            ViewBag.TapHienTai = int.Parse(lastPart);
            ViewBag.aliasTenPhim = aliasTenPhim;
            ViewBag.aliasPhimTap1 = aliasPhimTap1;

            List<Movie> listMovieHot = _context.Movies.Include(a => a.Categories).Where(a => a.Episode == 1 && a.Status == true).AsNoTracking().ToList();
            ViewBag.MovieHot = listMovieHot;

            List<Country> listCountry = _context.Countries.Take(9).AsNoTracking().ToList();
            List<Category> listCategory = _context.Categories.Take(15).AsNoTracking().ToList();
            ViewBag.Cat = listCategory;
            ViewBag.Country = listCountry;

            return View(model);
        }
    }
}
