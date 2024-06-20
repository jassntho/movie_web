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
using Movie_Web.Models;
using PagedList.Core;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminParametersController : Controller
    {
        private readonly MoviesContext _context;
        public INotyfService _notifyService { get; }

        public AdminParametersController(MoviesContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        // GET: Admin/AdminParameters

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.Page_Size;
            var lsParameters = _context.Parameters.OrderByDescending(x => x.ParameterId);
            PagedList<Parameter> models = new PagedList<Parameter>(lsParameters, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        // GET: Admin/AdminParameters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameter = await _context.Parameters
                .FirstOrDefaultAsync(m => m.ParameterId == id);
            if (parameter == null)
            {
                return NotFound();
            }

            return View(parameter);
        }

        // GET: Admin/AdminParameters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminParameters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParameterId,ParameterName,Value")] Parameter parameter)
        {
            if (ModelState.IsValid)
            {
               var parameter1 = _context.Parameters.FirstOrDefault(x => x.ParameterName == parameter.ParameterName);
                if(parameter1 == null)
                {
                    _context.Add(parameter);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Create Success", 2);
                  
                }
                else
                {
                    _notifyService.Error("Prameter is Exist", 2);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(parameter);
        }

        // GET: Admin/AdminParameters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameter = await _context.Parameters.FindAsync(id);
            if (parameter == null)
            {
                return NotFound();
            }
            return View(parameter);
        }

        // POST: Admin/AdminParameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParameterId,ParameterName,Value")] Parameter parameter)
        {
            if (id != parameter.ParameterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parameter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParameterExists(parameter.ParameterId))
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
            return View(parameter);
        }

        // GET: Admin/AdminParameters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameter = await _context.Parameters
                .FirstOrDefaultAsync(m => m.ParameterId == id);
            if (parameter == null)
            {
                return NotFound();
            }

            return View(parameter);
        }

        // POST: Admin/AdminParameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parameter = await _context.Parameters.FindAsync(id);
            if (parameter != null)
            {
                _context.Parameters.Remove(parameter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParameterExists(int id)
        {
            return _context.Parameters.Any(e => e.ParameterId == id);
        }
    }
}
