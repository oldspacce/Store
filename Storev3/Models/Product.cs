using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Storev3.Models
{
    [Authorize(Roles = "admin, customer")]
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            Productorders = new HashSet<Productorder>();
            Productstorages = new HashSet<Productstorage>();
            Userscores = new HashSet<Userscore>();
        }

        [Key]
        public Guid Productid { get; set; }
        public string Name { get; set; } = null!;
        [ForeignKey("Brandid")]
        [Required]
        public Guid Brandid { get; set; }
        [ForeignKey("Genreid")]
        [Required]
        public Guid Genreid { get; set; }
        [ForeignKey("Manufacturerid")]
        [Required]
        public Guid Manufacturerid { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public string Age { get; set; } = null;
        [Required]
        public int? Reservation { get; set; }
        [Required]
        public decimal Score { get; set; }
        [Required]
        public string? Image { get; set; } = null;

        public virtual Brand Brand { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual ICollection<Productorder> Productorders { get; set; }
        public virtual ICollection<Productstorage> Productstorages { get; set; }
        public virtual ICollection<Userscore> Userscores { get; set; }
    }
    public class ProductIndexData
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

    }
}
