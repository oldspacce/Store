using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.Debugger.Contracts.HotReload;
using Npgsql;
using Storev3.Models;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;

namespace Storev3.SeedData
{
    public class SeedData
    {
        public static void Initialize()
        {
            using (storeContext store = new storeContext())
            {
                store.Products.AddRange(
                    new Product
                    {
                        Productid = Guid.NewGuid(),
                        Name = "Пономарев",
                        Brandid = GetBrand("Star wars"),
                        Genreid = GetGenre("Стратегические игры"),
                        Manufacturerid = GetManufacturer("Hobby world"),
                        Cost = 8000,
                        Age = "12+",
                        Reservation = 0,
                        Score = 1
                    });
                store.SaveChanges();                
            }
        }
        public static Guid GetBrand(string name)
        {
            Guid brandid = Guid.Empty;
            using (storeContext store = new storeContext())
            {
                var brandContext = store.Brands;
                foreach (Brand brand in brandContext.Where(c => c.Name == name))
                {
                    brandid = brand.Brandid;
                    return brandid;
                }
            }
            return brandid;
        }
        public static Guid GetManufacturer(string name)
        {
            Guid manufacturerid = Guid.Empty;
            using (storeContext store = new storeContext())
            {
                var manContext = store.Manufacturers;
                foreach (Manufacturer manufacturer in manContext.Where(c => c.Name == name))
                {
                    manufacturerid = manufacturer.Manufacturerid;
                    return manufacturerid;
                }
            }
            return manufacturerid;
        }
        public static Guid GetGenre(string name)
        {
            Guid genreid = Guid.Empty;
            using (storeContext store = new storeContext())
            {
                var genreContext = store.Genres;
                foreach (Genre genre in genreContext.Where(c => c.Name == name))
                {
                    genreid = genre.Genreid;
                    return genreid;
                }
            }
            return genreid;
        }
        public static int Multi(int first, decimal second)
        {
            return first * (int)second;
        }
        public static int? ProductCount(Guid id)
        {
            using (storeContext context = new storeContext())
            {
                var productstorage = context.Productstorages.Where(p => p.Productid == id);
                var product = context.Products.Single(p => p.Productid == id);
                int? count = 0;
                foreach(Productstorage pd in productstorage)
                {
                    count += pd.Count;
                }
                count = count;
                return count;
            } 
        }
        public static int? ProductCountUser(Guid id, Guid user)
        {
            using (storeContext context = new storeContext())
            {
                var productstorage = context.Productstorages.Where(p => p.Productid == id);
                var productorder = context.Productorders.SingleOrDefault(p => p.Productid == id && p.Order.Userid == user && p.Order.Status == "Не оформлен");
                var product = context.Products.Single(p => p.Productid == id);
                int? count = 0;
                foreach (Productstorage pd in productstorage)
                {
                    count += pd.Count;
                }
                if (productorder != null)
                {
                    count = count - product.Reservation + productorder.Count;
                }
                else
                    count = count - product.Reservation;
                return count;
            }
        }
        public static Guid GetProductstorage(Guid productid, Guid storageid)
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
    }

    public enum agerestrictions
    {
        zero,
        six,
        eight,
        twelve,
        sixteen,
        eighteen
    }

}
