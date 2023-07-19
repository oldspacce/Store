using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storev3.Models;

namespace Storev3.Controllers
{
    public class PointofissuesController : Controller
    {
        private readonly storeContext _context;

        public PointofissuesController(storeContext context)
        {
            _context = context;
        }

        // GET: Pointofissues
        public async Task<IActionResult> Index()
        {
              return View(await _context.Pointofissues.ToListAsync());
        }

        // GET: Pointofissues/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Pointofissues == null)
            {
                return NotFound();
            }

            var pointofissue = await _context.Pointofissues
                .FirstOrDefaultAsync(m => m.Pointid == id);
            if (pointofissue == null)
            {
                return NotFound();
            }

            return View(pointofissue);
        }

        // GET: Pointofissues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pointofissues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pointid,Address")] Pointofissue pointofissue)
        {
            if (ModelState.IsValid)
            {
                pointofissue.Pointid = Guid.NewGuid();
                _context.Add(pointofissue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pointofissue);
        }

        // GET: Pointofissues/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Pointofissues == null)
            {
                return NotFound();
            }

            var pointofissue = await _context.Pointofissues.FindAsync(id);
            if (pointofissue == null)
            {
                return NotFound();
            }
            return View(pointofissue);
        }

        // POST: Pointofissues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Pointid,Address")] Pointofissue pointofissue)
        {
            if (id != pointofissue.Pointid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pointofissue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PointofissueExists(pointofissue.Pointid))
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
            return View(pointofissue);
        }

        // GET: Pointofissues/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Pointofissues == null)
            {
                return NotFound();
            }

            var pointofissue = await _context.Pointofissues
                .FirstOrDefaultAsync(m => m.Pointid == id);
            if (pointofissue == null)
            {
                return NotFound();
            }

            return View(pointofissue);
        }

        // POST: Pointofissues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Pointofissues == null)
            {
                return Problem("Entity set 'storeContext.Pointofissues'  is null.");
            }
            var pointofissue = await _context.Pointofissues.FindAsync(id);
            if (pointofissue != null)
            {
                _context.Pointofissues.Remove(pointofissue);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PointofissueExists(Guid id)
        {
          return _context.Pointofissues.Any(e => e.Pointid == id);
        }
    }
}
