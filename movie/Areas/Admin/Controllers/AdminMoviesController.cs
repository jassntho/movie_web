using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlog.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Movie_Web.Models;
using PagedList.Core;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminMoviesController : Controller
    {
        private readonly MoviesContext _context;
        private readonly IWebHostEnvironment _environment;
        public INotyfService _notifyService { get; }

        public AdminMoviesController(MoviesContext context, INotyfService notyfService, IWebHostEnvironment environment)
        {
            _context = context;
            _notifyService = notyfService;
            _environment = environment;
        }

        // GET: Admin/AdminMovies
        public IActionResult Index(int page = 1, int CountryId = 0)
        {
            var pageNumber = page;
            var pageSize = 20;

            List<Movie> lsMovies = new List<Movie>();
            if (CountryId != 0)
            {
                lsMovies = _context.Movies
                .Include(x => x.Type)
                .Where(x => x.CountryId == CountryId)
                .Include(x => x.Country)
                 .AsNoTracking()
                .OrderByDescending(x => x.MovieId).ToList();
            }
            else
            {
                lsMovies = _context.Movies
                .Include(x => x.Type)
                .AsNoTracking()
                .Include(x => x.Country)
                .OrderByDescending(x => x.MovieId).ToList();
            }
            PagedList<Movie> models = new PagedList<Movie>(lsMovies.AsQueryable(),pageNumber, pageSize);

            ViewBag.CurrentCountryId = CountryId;
            ViewBag.CurrentPage = pageNumber;
            ViewData["QuocGia"] = new SelectList(_context.Countries, "CountryId", "CountryName", CountryId);
            return View(models);
           
        }

        // GET: Admin/AdminMovies/Details/5


        public IActionResult Filtter(int CountryId = 0)
        {
            var url = $"/Admin/AdminMovies?CountryId={CountryId}";
            if(CountryId == 0)
            {
                url = $"/Admin/AdminMovies";
            }
            return Json(new {status = "success", redirectUrl = url});
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            // cay vai o

            var movie = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Type)
                .Include(m => m.Actors)
                .Include(m => m.Categories)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Admin/AdminMovies/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName");
            return View();
        }

        // POST: Admin/AdminMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,MovieName,Image,Description,Time,PublishedYear,MovieLink,TypeId,Episode,Viewed,Status,CountryId,Director,MovieLinkContingency,MovieLinkTrailer,ImageSlider,Alias,EpisodeLimit,DirectorAlias")] Movie movie, IFormFile? fImage, IFormFile? fImageSlider)
        {
            if (ModelState.IsValid)
            {
                
                movie.Alias = Utilities.SEOUrl(movie.MovieName) + "-" + Utilities.SEOUrl(Convert.ToString(movie.Episode));
                movie.DirectorAlias = Utilities.SEOUrl(movie.Director);
                var movie1 = _context.Movies.FirstOrDefault(x => x.Alias ==  movie.Alias && x.Episode == movie.Episode );
                if (movie1 == null)
                {
                    if (fImage != null)
                    {
                        string fileExtension = Path.GetExtension(fImage.FileName);
                        string newName = Utilities.SEOUrl(movie.MovieName) + "-image" + fileExtension;
                        movie.Image = await Utilities.UploadFile(fImage, @"MovieImage\", newName.ToLower());
                    }

                    if (fImageSlider != null)
                    {
                        string fileExtension = Path.GetExtension(fImageSlider.FileName);
                        string newName = Utilities.SEOUrl(movie.MovieName) + "-image-slider" + fileExtension;
                        movie.ImageSlider = await Utilities.UploadFile(fImageSlider, @"MovieImageSlider\", newName.ToLower());
                    }

                    _context.Add(movie);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Create Success", 2);
                }
                else
                {
                    _notifyService.Error("Movie đã tồn tại, Hãy xóa movie này trc khi thêm", 2);
                }
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", movie.CountryId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", movie.TypeId);
            return View(movie);
        }

        // GET: Admin/AdminMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", movie.CountryId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", movie.TypeId);
           
            return View(movie);
        }

        public async Task<IActionResult> AddActors(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
           
            ViewData["DienVien"] = new SelectList(_context.Actors, "ActorId", "ActorName");
            var max_actor_movie1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "Max_Actor_Movie").Value;
            int max_actor_movie = max_actor_movie1 == null ? 8 : max_actor_movie1.Value;
            ViewBag.Max_Actor_Movie = max_actor_movie;
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActors(int id, [Bind("MovieId,MovieName,Image,Description,Time,PublishedYear,MovieLink,TypeId,Episode,Viewed,Status,CountryId,Director,MovieLinkContingency,MovieLinkTrailer,ImageSlider,Alias,EpisodeLimit,DirectorAlias")] Movie movie, int Actors_1, int Actors_2, int Actors_3, int Actors_4, int Actors_5, int Actors_6, int Actors_7, int Actors_8)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(Actors_1 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_1);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }
                    if (Actors_2 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_2);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }
                    if (Actors_3 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_3);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }
                    if (Actors_4 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_4);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }
                    if (Actors_5 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_5);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }
                    if (Actors_6 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_6);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }
                    if (Actors_7 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_7);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }
                    if (Actors_8 != 0)
                    {
                        var actor = _context.Actors.FirstOrDefault(x => x.ActorId == Actors_8);
                        if (actor != null)
                        {
                            movie.Actors.Add((Actor)actor);
                        }
                    }

                    _context.Update(movie);
                    _notifyService.Success("AddActor Success", 2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            ViewData["DienVien"] = new SelectList(_context.Actors, "ActorId", "ActorName", movie.Actors);
            
            return View(movie);
        }

        public async Task<IActionResult> AddCat(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            var max_cat_movie1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "Max_Cat_Movie").Value;
            int max_cat_movie = max_cat_movie1 == null ? 8 : max_cat_movie1.Value;
            ViewBag.Max_Cat_Movie = max_cat_movie;
            return View(movie);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCat(int id, [Bind("MovieId,MovieName,Image,Description,Time,PublishedYear,MovieLink,TypeId,Episode,Viewed,Status,CountryId,Director,MovieLinkContingency,MovieLinkTrailer,ImageSlider,Alias,EpisodeLimit,DirectorAlias")] Movie movie, int Cat_1, int Cat_2, int Cat_3, int Cat_4, int Cat_5, int Cat_6, int Cat_7, int Cat_8)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Cat_1 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_1);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }
                    if (Cat_2 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_2);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }
                    if (Cat_3 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_3);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }
                    if (Cat_4 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_4);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }
                    if (Cat_5 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_5);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }
                    if (Cat_6 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_6);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }
                    if (Cat_7 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_7);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }
                    if (Cat_8 != 0)
                    {
                        var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Cat_8);
                        if (cat != null)
                        {
                            movie.Categories.Add((Category)cat);
                        }
                    }

                    _context.Update(movie);
                    _notifyService.Success("AddCat Success", 2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", movie.Categories);

            return View(movie);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,MovieName,Image,Description,Time,PublishedYear,MovieLink,TypeId,Episode,Viewed,Status,CountryId,Director,MovieLinkContingency,MovieLinkTrailer,ImageSlider,Alias,EpisodeLimit,DirectorAlias")] Movie movie, IFormFile? fImage, IFormFile? fImageSlider)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movie.Alias = Utilities.SEOUrl(movie.MovieName) + "-" + Utilities.SEOUrl(Convert.ToString(movie.Episode));
                    movie.DirectorAlias = Utilities.SEOUrl(movie.Director);
                    if (fImage != null)
                    {
                        string fileExtension = Path.GetExtension(fImage.FileName);
                        string newName = Utilities.SEOUrl(movie.MovieName) + "-image" + fileExtension;
                        movie.Image = await Utilities.UploadFile(fImage, @"MovieImage\", newName.ToLower());
                    }

                    if (fImageSlider != null)
                    {
                        string fileExtension = Path.GetExtension(fImageSlider.FileName);
                        string newName = Utilities.SEOUrl(movie.MovieName) + "-image-slider" + fileExtension;
                        movie.ImageSlider = await Utilities.UploadFile(fImageSlider, @"MovieImageSlider\", newName.ToLower());
                    }
                    _context.Update(movie);
                    _notifyService.Success("Edit Success", 2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", movie.CountryId);
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", movie.TypeId);
            return View(movie);
        }

        // POST: Admin/AdminMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
     

        // GET: Admin/AdminMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Type)
                .Include(m=> m.Actors)
                .Include(m=> m.Categories)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Admin/AdminMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            var movie = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.Type)
                .Include(m => m.Actors)
                .Include(m => m.Categories)
                .FirstOrDefaultAsync(m => m.MovieId == id);


            if (movie != null)
            {

               


                string Path1 = _environment.WebRootPath + "/images/MovieImage/" + movie.Image;
                if(System.IO.File.Exists(Path1))
                {
                    
                    System.IO.File.Delete(Path1);
                }
               
                string Path2 = _environment.WebRootPath + "/images/MovieImageSlider/" + movie.ImageSlider;
                if (System.IO.File.Exists(Path2))
                {
                    System.IO.File.Delete(Path2);
                }
                
                


                movie.Actors.Clear();

                movie.Categories.Clear();

                _notifyService.Success("Delete Success", 2);

                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
