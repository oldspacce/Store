using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storev3.SeedData;
using Microsoft.CodeAnalysis;
using Storev3.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Storev3.Controllers
{
    public class ProductsController : Controller
    {
        private readonly storeContext _context;
        IWebHostEnvironment _appEnvironment;

        public ProductsController(storeContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.Products.Include(p => p.Brand).Include(p => p.Genre).Include(p => p.Manufacturer);
            return View(await storeContext.ToListAsync());
        }
        
        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = new ProductIndexData();
            product.Products = _context.Products.Where(m => m.Productid == id)
                .Include(i => i.Brand)
               .Include(i => i.Genre)
               .Include(i => i.Manufacturer);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult AddCount(Guid id)
        {
            ViewData["Productid"] = new SelectList(_context.Products.Where(p => p.Productid == id), "Productid", "Name");
            ViewData["Storageid"] = new SelectList(_context.Storages, "Storageid", "Address");
            return View(nameof(AddCount));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCount([Bind("Productstorageid,Productid,Storageid,Count")] Productstorage productstorage)
        {
            if (!ProductStorageExists(productstorage.Productid))
            {
                productstorage.Productstorageid = Guid.NewGuid();
                _context.Add(productstorage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productstorage.Productstorageid = GetProductstorage(productstorage.Productid, productstorage.Storageid);
                _context.Productstorages.Attach(productstorage);
                _context.Entry(productstorage).Property(x => x.Count).IsModified = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Age", productstorage.Productid);
            //ViewData["Storageid"] = new SelectList(_context.Storages, "Storageid", "Address", productstorage.Storageid);
            //return View(productstorage);
        }
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Brandid"] = new SelectList(_context.Brands, "Brandid", "Name");
            ViewData["Genreid"] = new SelectList(_context.Genres, "Genreid", "Name");
            ViewData["Manufacturerid"] = new SelectList(_context.Manufacturers, "Manufacturerid", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Productid,Name,Brandid,Genreid,Manufacturerid,Cost,Age,Reservation,Score")] Product product, IFormFile Image)
        {
            if (Image != null)
            {
                string path = "/Images/" + Image.FileName;
                product.Image = path;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
            }
            product.Productid = Guid.NewGuid();
            _context.Add(product);
            await _context.SaveChangesAsync();
            Guid id = product.Productid;
            return RedirectToAction("AddCount", new { id });
            //ViewData["Brandid"] = new SelectList(_context.Brands, "Brandid", "Name", product.Brandid);
            //ViewData["Genreid"] = new SelectList(_context.Genres, "Genreid", "Name", product.Genreid);
            //ViewData["Manufacturerid"] = new SelectList(_context.Manufacturers, "Manufacturerid", "Name", product.Manufacturerid);
            //return View(product);
        }

        // GET: Products/Edit/5

        public async Task<IActionResult> EditScore(Guid userid, Guid productid)
        {
            //var context = _context.Productorders.Include()
            var userscore = await _context.Userscores.SingleOrDefaultAsync(u => u.Userid == userid && u.Productid == productid);
            if (userscore == null)
            {
                userscore = new Userscore();
                userscore.Userscoreid = Guid.NewGuid();
                userscore.Userid = userid;
                userscore.Productid = productid;
                userscore.Score = 1;
                _context.Add(userscore);
                _context.SaveChanges();
            }
            return PartialView(userscore);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScore([Bind("Userscoreid", "Userid", "Productid", "Score")] Userscore userscore)
        {
            _context.Userscores.Attach(userscore);
            _context.Entry(userscore).Property(x => x.Score).IsModified = true;
            Guid id = userscore.Productid;
            _context.SaveChanges();
            return RedirectToAction("Details", new { id });
        }
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Brandid"] = new SelectList(_context.Brands, "Brandid", "Name", product.Brandid);
            ViewData["Genreid"] = new SelectList(_context.Genres, "Genreid", "Name", product.Genreid);
            ViewData["Manufacturerid"] = new SelectList(_context.Manufacturers, "Manufacturerid", "Name", product.Manufacturerid);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Productid,Name,Brandid,Genreid,Manufacturerid,Cost,Age,Reservation,Score,Image")] Product product, IFormFile Image)
        {
            if (id != product.Productid)
            {
                return NotFound();
            }
                try
                {
                if(Image != null)
                {
                    string path = "/Images/" + Image.FileName;
                    string oldpath = product.Image;
                    product.Image = path;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }
                    if(oldpath != null)
                    {
                        FileInfo fileInfo= new FileInfo(_appEnvironment.WebRootPath + oldpath);
                        fileInfo.Delete();
                    }
                }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Productid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //ViewData["Brandid"] = new SelectList(_context.Brands, "Brandid", "Name", product.Brandid);
            //ViewData["Genreid"] = new SelectList(_context.Genres, "Genreid", "Name", product.Genreid);
            //ViewData["Manufacturerid"] = new SelectList(_context.Manufacturers, "Manufacturerid", "Name", product.Manufacturerid);
            //return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Genre)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(m => m.Productid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'storeContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddCart(Guid id, [Bind("Productorderid, Productid, Orderid, Count")] Productorder productorder)
        {
            if (!ProductOrderExists(id))
            {
                productorder.Count = 1;
                productorder.Productorderid = Guid.NewGuid();
                productorder.Productid = id;
                if (!OrderExists(Guid.Parse(User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).Single())))
                {
                    Order order = new Order();
                    order.Orderid = Guid.NewGuid();
                    order.Status = "Не оформлен";
                    order.Userid = Guid.Parse(@User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault());
                    order.Fullcost = 0;
                    order.Pointid = Guid.Parse("01fc6c79-9d9e-407f-8e85-f53194945edd");
                    order.Point = null;
                    DateTime datedelivery = DateTime.Now;
                    datedelivery = datedelivery.AddDays(4);
                    order.Datedelivery = DateOnly.FromDateTime(datedelivery);
                    _context.Add(order);
                    _context.SaveChanges();
                }
                productorder.Orderid = GetOrderid(User.Claims.Where(c => c.Type == "Id")
                        .Select(c => c.Value).Single());
                _context.Add(productorder);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id });
            }
            else
            {
                var pdexists = _context.Productorders.Where(c => c.Productid == id);
                foreach (Productorder pd in pdexists)
                    pd.Count += 1;
                _context.SaveChanges();
                return RedirectToAction("Details", new { id });
            }
        }

        private bool ProductExists(Guid id)
        {
          return _context.Products.Any(e => e.Productid == id);
        }
        private bool ProductOrderExists(Guid id)
        {
            string user = @User.Claims
                .Where(c => c.Type == "Id")
                .Select(c => c.Value).Single();
            if(_context.Orders.Any(d => d.Userid == id))
            {
                var order = GetOrderid(user);
                return _context.Productorders.Any(p => p.Orderid == order && p.Productid == id);
            }
            else
            {
                return false;
            }
        }
        private bool ProductStorageExists(Guid id)
        {
            return _context.Productstorages.Any(p => p.Productid == id);
        }
        private bool OrderExists(Guid id)
        {
            string user = @User.Claims
                .Where(c => c.Type == "Id")
                .Select(c => c.Value).Single();
            return _context.Orders.Any(d => d.Userid.ToString() == user && d.Status == "Не оформлен");
        }
        private static Guid GetProductstorage(Guid productid, Guid storageid)
        {
            using (storeContext store = new storeContext())
            {
                var PsContext = store.Productstorages;
                foreach (Productstorage ps in PsContext.Where(p => p.Productid == productid
                && p.Storageid == storageid))
                {
                    return ps.Productstorageid;
                }
            }
            return Guid.Empty;
        }
        public static Guid GetOrderid(string name)
        {
            Guid Orderid = Guid.Empty;
            using (storeContext store = new storeContext())
            {
                foreach (Order order in store.Orders.Where(d => d.Userid.ToString() == name && d.Status == "Не оформлен"))
                {
                    Orderid = order.Orderid;
                    return Orderid;
                }
                return Orderid;
            }

        }
        public static bool UserscoreExists(string userid, Guid productid)
        {
            using (storeContext store = new storeContext())
            {
                Guid userguid = Guid.Parse(userid);
                return store.Userscores.Any(u => u.Userid == userguid && u.Productid == productid);
            }
        }
    }
}
