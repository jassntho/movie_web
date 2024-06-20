using AppBlog.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Movie_Web.Helpers;
using Movie_Web.Models;
using Newtonsoft.Json;
using PagedList.Core;
using System.Security.Claims;



namespace Movie_Web.Controllers
{
    public class TypeController : Controller
    {
      
        private readonly MoviesContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TypeController(MoviesContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("{Alias}/page={page}", Name = "KieuPhim")]
        public IActionResult Index(string Alias, int? page)
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
            if (Alias == "Admin")
            {
                return Redirect("~/Admin/Home");
            }

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "Page_Size").Value;
            //int pageSize = pageSize1 == null ? 1 : pageSize1.Value;
            int pageSize = 6;
            // Chi dua ra Alias o tap 1 thoi
            var kieuphim = _context.Types.FirstOrDefault(x => x.TypeAlias == Alias);
            List<Movie> lsMovies = new List<Movie>();



            if(kieuphim != null)
            {
                lsMovies = _context.Movies.Include(x => x.Type).Where(x => x.Type.TypeAlias == Alias && x.Episode == 1).AsNoTracking().ToList();
                int CountMovie = _context.Movies.Include(x => x.Type).Where(x => x.Type.TypeAlias == Alias && x.Episode == 1).AsNoTracking().Count();
                int SoTrangMax = (int)Math.Ceiling((double)CountMovie / pageSize);
                ViewBag.SoTrangMax = SoTrangMax;
            }
            else { 
                        return NotFound();
                        //return RedirectToAction("Index", "Home");
               
            }

            List<Country> listCountry = _context.Countries.Take(9).AsNoTracking().ToList();
            List<Category> listCategory = _context.Categories.Take(15).AsNoTracking().ToList();
            ViewBag.Cat = listCategory;
            ViewBag.Country = listCountry;
          
            string alias = Alias;
            ViewBag.Alias = alias;
           

          
            PagedList<Movie> models = new PagedList<Movie>(lsMovies.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            
            return View(models);
        }

        [Route("the-loai/{Alias}/page={page}", Name = "TheLoai")]
        public IActionResult TheLoai(string Alias, int? page)
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

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "Page_Size").Value;
            //int pageSize = pageSize1 == null ? 1 : pageSize1.Value;
            int pageSize = 6;


            List<Movie> lsMovies = new List<Movie>();
            
            var theloai = _context.Categories.FirstOrDefault(x => x.CategoriesAlias == Alias);
            if (theloai != null)
            {
                lsMovies = _context.Movies.Include(x => x.Categories).Where(x => x.Episode == 1 &&  x.Categories.Any(y => y.CategoriesAlias == Alias)).ToList();
                int CountMovie = _context.Movies.Include(x => x.Categories).Where(x => x.Episode == 1 && x.Categories.Any(y => y.CategoriesAlias == Alias)).Count();
                
                int SoTrangMax = (int)Math.Ceiling((double)CountMovie / pageSize);
                ViewBag.SoTrangMax = SoTrangMax;
                string tenTheLoai = theloai.CategoryName.ToString();
                ViewBag.tenTheLoai = tenTheLoai;    
            }
            else
            {
                return NotFound();
                //return RedirectToAction("Index", "Home");
            }
            List<Country> listCountry = _context.Countries.Take(9).AsNoTracking().ToList();
            List<Category> listCategory = _context.Categories.Take(15).AsNoTracking().ToList();
            ViewBag.Cat = listCategory;
            ViewBag.Country = listCountry;

            string alias = Alias;
            ViewBag.Alias = alias;




            PagedList<Movie> models = new PagedList<Movie>(lsMovies.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }
        [Route("quoc-gia/{Alias}/page={page}", Name = "QuocGia")]
        public IActionResult QuocGia(string Alias, int? page)
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

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "Page_Size").Value;
            //int pageSize = pageSize1 == null ? 1 : pageSize1.Value;
            int pageSize = 6;


            List<Movie> lsMovies = new List<Movie>();

            var quocgia = _context.Countries.FirstOrDefault(x => x.CountryAlias == Alias);
            if (quocgia != null)
            {
                lsMovies = _context.Movies.Include(x => x.Country).Where(x => x.Country.CountryAlias == Alias && x.Episode == 1).AsNoTracking().ToList();
                int CountMovie = _context.Movies.Include(x => x.Country).Where(x => x.Country.CountryAlias == Alias && x.Episode == 1).AsNoTracking().Count();
                int SoTrangMax = (int)Math.Ceiling((double)CountMovie / pageSize);
                ViewBag.SoTrangMax = SoTrangMax;
                string tenQuocGia = quocgia.CountryName.ToString();
                ViewBag.tenQuocGia = tenQuocGia;
            }
            else
            {
                return NotFound();
                //return RedirectToAction("Index", "Home");
            }

            List<Country> listCountry = _context.Countries.Take(9).AsNoTracking().ToList();
            List<Category> listCategory = _context.Categories.Take(15).AsNoTracking().ToList();
            ViewBag.Cat = listCategory;
            ViewBag.Country = listCountry;

            string alias = Alias;
            ViewBag.Alias = alias;





            PagedList<Movie> models = new PagedList<Movie>(lsMovies.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        [Route("tim-kiem/", Name = "TimKiem")]
        public IActionResult TimKiem()
        {

            string searchValue = HttpContext.Request.Query["search"];

            // Kiểm tra xem tham số có tồn tại không
            if (!string.IsNullOrEmpty(searchValue))
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


                string aliasSearch = Utilities.SEOUrl(searchValue);



                List<Movie> models = new List<Movie>();

                //models = _context.Movies.Where(x => x.Alias.Contains(aliasSearch) && x.Episode == 1).ToList();
                

                List<Country> listCountry = _context.Countries.Take(9).AsNoTracking().ToList();
                List<Category> listCategory = _context.Categories.Take(15).AsNoTracking().ToList();
                ViewBag.Cat = listCategory;
                ViewBag.Country = listCountry;

                string alias = searchValue;
                ViewBag.Alias = alias;

               

                string webRootPath = _webHostEnvironment.WebRootPath;
                string jsonFilePath = webRootPath+ "/File/moviename.json";

                List<string> movies = new List<string>();

                try
                {
                    string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                    var json = JsonConvert.DeserializeObject<MovieList>(jsonContent);

                    if (json != null && json.MovieName != null)
                    {
                        movies = json.MovieName.ToList();

                        string searchText =  searchValue;
                        int thresholdScore = 45;
                        List<string> similarMovies = Search.FindSimilarProducts(searchText, movies, thresholdScore);

                        if (similarMovies.Count > 0)
                        {
                            foreach (var movie in similarMovies)
                            {
                                var bienMovie = _context.Movies.FirstOrDefault(x => x.MovieName == movie);
                                models.Add(bienMovie);
                            }
                            ViewBag.similarMovies = similarMovies;
                            ViewBag.CountPhim = similarMovies.Count;
                        }
                        else
                        {
                            ViewBag.similarMovies = similarMovies;
                            ViewBag.CountPhim = 0;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy dữ liệu phim.");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi đọc tệp JSON: {ex.Message}");

                }
                




                return View(models);
            }

            return RedirectToAction("Index", "Home");

        }

        
        
    }
}