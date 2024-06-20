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
    public class AdminTypesController : Controller
    {
        private readonly MoviesContext _context;
        public INotyfService _notifyService { get; }

        public AdminTypesController(MoviesContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        // GET: Admin/AdminTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Types.ToListAsync());
        }

        // GET: Admin/AdminTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // GET: Admin/AdminTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName,TypeAlias")] Movie_Web.Models.Type @type)
        {
            if (ModelState.IsValid)
            {
                
                type.TypeAlias = Utilities.SEOUrl(type.TypeName);
                var type1 = _context.Types.FirstOrDefault(x => x.TypeAlias == type.TypeAlias);
                if(type1 == null)
                {
                    _context.Add(@type);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Create Success", 2);
                }
                else
                {
                    _notifyService.Error("Type is exist", 2);
                }
               
                return RedirectToAction(nameof(Index));
            }
            return View(@type);
        }

        // GET: Admin/AdminTypes/Edit/5



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }
            return View(@type);
        }

        // POST: Admin/AdminTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName,TypeAlias")] Movie_Web.Models.Type @type)
        {
            if (id != @type.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    type.TypeAlias = Utilities.SEOUrl(type.TypeName);
                    _context.Update(@type);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Edit Success", 2);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(@type.TypeId))
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
            return View(@type);
        }

        // GET: Admin/AdminTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Types
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // POST: Admin/AdminTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @type = await _context.Types.FindAsync(id);
            if (@type != null)
            {
                _context.Types.Remove(@type);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Delete Success", 2);
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(int id)
        {
            return _context.Types.Any(e => e.TypeId == id);
        }
    }
}
