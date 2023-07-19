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
    public class StoragesController : Controller
    {
        private readonly storeContext _context;

        public StoragesController(storeContext context)
        {
            _context = context;
        }

        // GET: Storages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Storages.ToListAsync());
        }

        // GET: Storages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Storages == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages
                .FirstOrDefaultAsync(m => m.Storageid == id);
            if (storage == null)
            {
                return NotFound();
            }

            return View(storage);
        }

        // GET: Storages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Storages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Storageid,Address")] Storage storage)
        {
            if (ModelState.IsValid)
            {
                storage.Storageid = Guid.NewGuid();
                _context.Add(storage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storage);
        }

        // GET: Storages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Storages == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages.FindAsync(id);
            if (storage == null)
            {
                return NotFound();
            }
            return View(storage);
        }

        // POST: Storages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Storageid,Address")] Storage storage)
        {
            if (id != storage.Storageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorageExists(storage.Storageid))
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
            return View(storage);
        }

        // GET: Storages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Storages == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages
                .FirstOrDefaultAsync(m => m.Storageid == id);
            if (storage == null)
            {
                return NotFound();
            }

            return View(storage);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Storages == null)
            {
                return Problem("Entity set 'storeContext.Storages'  is null.");
            }
            var storage = await _context.Storages.FindAsync(id);
            if (storage != null)
            {
                _context.Storages.Remove(storage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageExists(Guid id)
        {
            return _context.Storages.Any(e => e.Storageid == id);
        }
    }
}
