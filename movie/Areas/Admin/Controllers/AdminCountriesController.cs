using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlog.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Web.Models;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCountriesController : Controller
    {
        private readonly MoviesContext _context;
        public INotyfService _notifyService { get; }

        public AdminCountriesController(MoviesContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        // GET: Admin/AdminCountries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries.ToListAsync());
        }

        // GET: Admin/AdminCountries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Admin/AdminCountries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCountries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryId,CountryName,CountryAlias")] Country country)
        {
            if (ModelState.IsValid)
            {
                country.CountryAlias = Utilities.SEOUrl(country.CountryName);
                var country1 = _context.Countries.FirstOrDefault(x => x.CountryAlias == country.CountryAlias);
                if (country1 == null) {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Create Success", 2);
                }
                else
                {
                    _notifyService.Error("Country is exist", 2);
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Admin/AdminCountries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Admin/AdminCountries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CountryId,CountryName,CountryAlias")] Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    country.CountryAlias = Utilities.SEOUrl(country.CountryName);
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Edit Success", 2);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryId))
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
            return View(country);
        }

        // GET: Admin/AdminCountries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Admin/AdminCountries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Delete Success", 2);
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.CountryId == id);
        }
    }
}
