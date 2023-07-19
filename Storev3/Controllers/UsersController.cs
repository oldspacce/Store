using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Storev3.SeedData;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Cryptography;
using System.Text;
using Storev3.Models;

namespace Storev3.Controllers
{
    public class UsersController : Controller
    {
        private readonly storeContext _context;

        public UsersController(storeContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userid,Surname,Name,Patronymic,Login,Password,Phone,Role")] User user)
        {
            user.Role = 2;
            user.Password = GetHash(user.Password);
            user.Userid = Guid.NewGuid();
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Userid,Surname,Name,Patronymic,Login,Password,Phone")] User user)
        {
            if (id != user.Userid)
            {
                return NotFound();
            }
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Userid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", new { id });
        }
        public async Task<IActionResult> DeletefromCart(Guid id)
        {
            if (_context.Productorders == null)
            {
                return Problem("Entity set 'storeContext.Products'  is null.");
            }
            var order = ProductsController.GetOrderid(@User.Claims
                .Where(c => c.Type == "Id")
                .Select(c => c.Value).Single());
            var productorder = _context.Productorders.Where(c => c.Productid == id && c.Orderid == order);
            if (productorder != null)
            {
                foreach (Productorder pd in productorder)
                    _context.Productorders.Remove(pd);
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
            Guid guid = Guid.Parse(User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).Single());
            return RedirectToAction("Cart", new { guid });
        }
        // GET: Users/Cart/5
        public async Task<IActionResult> Cart(Guid guid)
        {
            //var cartContext = _context.Orders.Where(c => c.Userid == user.Userid).Include(p => p.Productorders).ThenInclude(p => p.Product);
            CartModel viewModel = new CartModel();
            Guid orderid = ProductsController.GetOrderid(guid.ToString());
            viewModel.Order1 = _context.Orders.Where(c => c.Userid == guid && c.Status == "Не оформлен").Include(p => p.Productorders).ThenInclude(p => p.Product);
            viewModel.Productorder1 = _context.Productorders.Where(c => c.Orderid == orderid).Include(p => p.Product);
            foreach (var item in viewModel.Order1)
            {
                viewModel.ConfOrder = item;
            }
            return View(viewModel);
        }
        public async Task<IActionResult> EditCount(Guid id)
        {
            if (id == null || _context.Productorders == null)
            {
                return NotFound();
            }
            //var context = _context.Productorders.Include()
            var productorder = await _context.Productorders.FindAsync(id);
            if (productorder == null)
            {
                return NotFound();
            }
            return PartialView(productorder);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCount(Guid id, [Bind("Productorderid", "Productid", "Orderid", "Count")] Productorder productorder)
        {
            if (id != productorder.Productorderid)
            {
                return NotFound();
            }
            Guid guid = Guid.Parse(User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).Single());
            if (SeedData.SeedData.ProductCountUser(productorder.Productid, guid) < productorder.Count)
            {
                List<Guid> product = new List<Guid>();
                product.Add(productorder.Productid);
                return RedirectToAction("ProductOver", new { listproduct = product });
            }
            _context.Productorders.Attach(productorder);
            _context.Entry(productorder).Property(x => x.Count).IsModified = true;
            await _context.SaveChangesAsync();            
            return RedirectToAction("Cart", new { guid });
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'storeContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MakingAnOrder(Guid guid)
        {
            Guid orderid = ProductsController.GetOrderid(guid.ToString());
            var listproduct = new List<Guid>();
            if (StorageCheck(orderid, out listproduct))
            {
                CartModel viewModel = new CartModel();
                viewModel.Order1 = _context.Orders.Where(c => c.Userid == guid && c.Status == "Не оформлен").Include(p => p.Productorders).ThenInclude(p => p.Product);
                foreach (var item in viewModel.Order1)
                {
                    viewModel.ConfOrder = item;
                }
                viewModel.Productorder1 = _context.Productorders.Where(c => c.Orderid == orderid).Include(p => p.Product);
                ViewData["Pointofissue"] = new SelectList(_context.Pointofissues, "Pointid", "Address");
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Productover", new { listproduct });
            }
        }
        public async Task<IActionResult> ProductOver(List<Guid> listproduct)
        {
            Dictionary<string, Guid> name = new Dictionary<string, Guid>();
            foreach (var item in listproduct)
            {
                var products = _context.Products.Single(p => p.Productid == item);
                name.Add(products.Name, products.Productid);
            }
            return View(name);
        }
        public async Task<IActionResult> PaymentAnOrder(Guid id)
        {
            Order order = await _context.Orders.FindAsync(id);
            order.Status = "Оплачен";
            DateTime datedelivery = DateTime.Now;
            datedelivery = datedelivery.AddDays(4);
            order.Datedelivery = DateOnly.FromDateTime(datedelivery);
            _context.Orders.Attach(order);
            _context.Entry(order).Property(x => x.Pointid).IsModified = true;
            _context.Entry(order).Property(x => x.Status).IsModified = true;
            await _context.SaveChangesAsync();
            Guid guid = Guid.Parse(User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).Single());
            return RedirectToAction("Cart", new { guid });
        }
        public async Task<IActionResult> UserOrders(Guid id)
        {
            var ordercontext = _context.Orders.Include(p => p.Point).Where(x => x.Userid == id && x.Status != "Не оформлен");
            return View(await ordercontext.ToListAsync());
        }
        public ActionResult EditRole(Guid id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = _context.Users.Find(id);
            ViewData["Roleid"] = new SelectList(_context.Roles, "Id", "Name");
            if(user != null)
            return PartialView(user);
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(Guid id, [Bind("Userid,Surname,Name,Patronymic,Login,Password,Phone,Role")] User user)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            _context.Users.Attach(user);
            _context.Entry(user).Property(x => x.Role).IsModified = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DetailsOrder(Guid id)
        {
            if(id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = new CartModel();
            order.ConfOrder = _context.Orders.Find(id);
            order.Productorder1 = _context.Productorders.Where(x => x.Orderid == id).Include(p => p.Product); ;
            if(order.ConfOrder == null)
            {
                return NotFound();
            }            
            return View(order);
        }
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Userid == id);
        }
        private bool StorageCheck(Guid guid, out List<Guid> product1)
        {
            int count = 0;
            var product = new Dictionary<Guid, int>();
            var listproduct = new List<Guid>();
            var productstorage = _context.Productstorages; 
            foreach(var item in productstorage)
            {
                product.Add(item.Productid, Convert.ToInt32(SeedData.SeedData.ProductCount(item.Productid)));
            }
            foreach (var ps in _context.Productorders.Where(po => po.Orderid == guid))
            {
                if (product.ContainsKey(ps.Productid))
                {
                    if (product[ps.Productid] <= ps.Count)
                    {
                        listproduct.Add(ps.Productid);
                        count++;
                    }
                }
            }
                if (count > 0)
            {
                product1 = listproduct;
                return false;
            }
            else
            {
                listproduct.Add(Guid.Empty);
                product1 = listproduct;
                return true;
            }

        }
        private Dictionary<string, int> ProductWho(Guid guid)
        {
            var product1 = _context.Products.Single(p => p.Productid == guid);
            int count = Convert.ToInt32(SeedData.SeedData.ProductCount(guid));
            Dictionary<string, int> product = new Dictionary<string, int>();
            product.Add(product1.Name, count);
            return product;
        }
        public string GetHash(string password)
        {
            var ps = MD5.Create();
            var hash = ps.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}
