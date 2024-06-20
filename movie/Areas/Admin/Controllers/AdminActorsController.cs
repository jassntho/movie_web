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
using Microsoft.VisualBasic.FileIO;
using Movie_Web.Models;
using PagedList.Core;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminActorsController : Controller
    {
        private readonly MoviesContext _context;
        public INotyfService _notifyService { get; }

        public AdminActorsController(MoviesContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        // GET: Admin/AdminActors

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == "Page_Size").Value;
            int pageSize = pageSize1 == null ? 1 : pageSize1.Value;
            var lsActors = _context.Actors.OrderByDescending(x => x.ActorId);
            PagedList<Actor> models = new PagedList<Actor>(lsActors, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
       

        // GET: Admin/AdminActors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // GET: Admin/AdminActors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminActors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,ActorName,ActorAlias")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                actor.ActorAlias = Utilities.SEOUrl(actor.ActorName);
                var actor1 = _context.Actors.FirstOrDefault(x => x.ActorAlias == actor.ActorAlias);
                if (actor1 == null)
                {
                    _context.Add(actor);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Create Success", 2);
                }
                else
                {
                    _notifyService.Error("Actor is exist", 2);
                }
               
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Admin/AdminActors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Admin/AdminActors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,ActorName,ActorAlias")] Actor actor)
        {
            if (id != actor.ActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    actor.ActorAlias = Utilities.SEOUrl(actor.ActorName);
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Edit Success", 2);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ActorId))
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
            return View(actor);
        }

        // GET: Admin/AdminActors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Admin/AdminActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Delete Success", 2);
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.ActorId == id);
        }
    }
}
