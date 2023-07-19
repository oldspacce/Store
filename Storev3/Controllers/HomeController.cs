using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storev3.Models;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Storev3.Controllers;
using System.Security.Cryptography;
using System.Text;

namespace Storev3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly storeContext _context;
        public HomeController(ILogger<HomeController> logger, storeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string Searchstring, string Brandstring, string Genrestring)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["CostSortParam"] = sortOrder == "Cost_esc" ? "Cost_desc" : "Cost_esc";
            ViewData["ScoreSortParam"] = sortOrder == "Score_esc" ? "Score_desc" : "Score_esc";
            ViewData["CurrentSearch"] = Searchstring;
            ViewData["BrandFilter"] = Brandstring;
            ViewData["GenreFilter"] = Genrestring;
            var viewModel = new ProductIndexData();
            viewModel.Products = _context.Products.Include(i => i.Brand)
                .Include(i => i.Genre)
                .Include(i => i.Manufacturer);
            viewModel.Brands = _context.Brands;
            viewModel.Genres = _context.Genres;
            if(!String.IsNullOrEmpty(Genrestring) && String.IsNullOrEmpty(Searchstring))
            {
                viewModel.Products = viewModel.Products.Where(p => p.Genre.Name == Genrestring);
            }
            if (!String.IsNullOrEmpty(Brandstring) && String.IsNullOrEmpty(Searchstring))
            {
                viewModel.Products = viewModel.Products.Where(p => p.Brand.Name == Brandstring);
            }
            if (!String.IsNullOrEmpty(Brandstring) && !String.IsNullOrEmpty(Genrestring) && String.IsNullOrEmpty(Searchstring))
            {
                viewModel.Products = viewModel.Products.Where(p => p.Brand.Name == Brandstring && p.Genre.Name == Genrestring);
            }
            if (!String.IsNullOrEmpty(Searchstring))
            {
                viewModel.Products = viewModel.Products.Where(p => p.Name.Contains(Searchstring));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    viewModel.Products = viewModel.Products.OrderByDescending(i => i.Name);
                    break;
                case "Cost_desc":
                    viewModel.Products = viewModel.Products.OrderByDescending(i => i.Cost);
                    break;
                case "Cost_esc":
                    viewModel.Products = viewModel.Products.OrderBy(i => i.Cost);
                    break;
                case "Score_esc":
                    viewModel.Products = viewModel.Products.OrderBy(i => i.Score);
                    break;
                case "Score_desc":
                    viewModel.Products = viewModel.Products.OrderByDescending(i => i.Score);
                    break;
                default:
                    viewModel.Products = viewModel.Products.OrderBy(i => i.Name);
                    break;
            }
            return View(viewModel);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Login == model.Login &&
                u.Password == GetHash(model.Password));
                if(user != null)
                {
                    string role = GetRole(user.Role);
                    await Autentificate(model, user.Userid, role);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неверный логин или пароль");
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task Autentificate(LoginModel model, Guid guid, string role)
        {
            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
            new Claim("Id", guid.ToString())};

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public static string GetRole(int? roleid)
        {
            using (storeContext store = new storeContext())
            {
                var rolecontext = store.Roles;
                foreach(Role role in rolecontext.Where(c => c.Id == roleid))
                {
                    return role.Name;
                }
            }
            return "customer";
        }
        public string GetHash(string password)
        {
            var ps = MD5.Create();
            var hash = ps.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
        //    public static string GetRolewithid(string guid)
        //    {
        //        Guid id = Guid.Parse(guid);
        //        using (storeContext store = new storeContext())
        //        {
        //            var user = store.Users.FirstOrDefault(u => u.Userid == id);
        //            var roleContext = store.Roles;
        //            foreach (Role role in roleContext.Where(c => c.Id == user.Role)) {
        //                return role.Name;
        //                    }
        //        }
        //        return "customer";
        //    }
    }
}