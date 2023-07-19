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
    public class ProductstoragesController : Controller
    {
        private readonly storeContext _context;

        public ProductstoragesController(storeContext context)
        {
            _context = context;
        }

        // GET: Productstorages
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.Productstorages.Include(p => p.Product).Include(p => p.Storage);
            return View(await storeContext.ToListAsync());
        }

        // GET: Productstorages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Productstorages == null)
            {
                return NotFound();
            }

            var productstorage = await _context.Productstorages
                .Include(p => p.Product)
                .Include(p => p.Storage)
                .FirstOrDefaultAsync(m => m.Productstorageid == id);
            if (productstorage == null)
            {
                return NotFound();
            }

            return View(productstorage);
        }

        // GET: Productstorages/Create
        public IActionResult Create()
        {
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Name");
            ViewData["Storageid"] = new SelectList(_context.Storages, "Storageid", "Address");
            return View();
        }

        // POST: Productstorages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Productstorageid,Productid,Storageid,Count")] Productstorage productstorage)
        {
            if (ModelState.IsValid)
            {
                productstorage.Productstorageid = Guid.NewGuid();
                _context.Add(productstorage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Age", productstorage.Productid);
            ViewData["Storageid"] = new SelectList(_context.Storages, "Storageid", "Address", productstorage.Storageid);
            return View(productstorage);
        }

        // GET: Productstorages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Productstorages == null)
            {
                return NotFound();
            }

            var productstorage = await _context.Productstorages.FindAsync(id);
            if (productstorage == null)
            {
                return NotFound();
            }
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Age", productstorage.Productid);
            ViewData["Storageid"] = new SelectList(_context.Storages, "Storageid", "Address", productstorage.Storageid);
            return View(productstorage);
        }

        // POST: Productstorages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Productstorageid,Productid,Storageid,Count")] Productstorage productstorage)
        {
            if (id != productstorage.Productstorageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productstorage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductstorageExists(productstorage.Productstorageid))
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
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Age", productstorage.Productid);
            ViewData["Storageid"] = new SelectList(_context.Storages, "Storageid", "Address", productstorage.Storageid);
            return View(productstorage);
        }

        // GET: Productstorages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Productstorages == null)
            {
                return NotFound();
            }

            var productstorage = await _context.Productstorages
                .Include(p => p.Product)
                .Include(p => p.Storage)
                .FirstOrDefaultAsync(m => m.Productstorageid == id);
            if (productstorage == null)
            {
                return NotFound();
            }

            return View(productstorage);
        }

        // POST: Productstorages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Productstorages == null)
            {
                return Problem("Entity set 'storeContext.Productstorages'  is null.");
            }
            var productstorage = await _context.Productstorages.FindAsync(id);
            if (productstorage != null)
            {
                _context.Productstorages.Remove(productstorage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductstorageExists(Guid id)
        {
          return _context.Productstorages.Any(e => e.Productstorageid == id);
        }
    }
}
