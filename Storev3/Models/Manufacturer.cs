using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Storev3.Models
{
    [Authorize(Roles = "admin")]
    [Table("Manufacturer")]
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public Guid Manufacturerid { get; set; }
        [Required]
        public string Name { get; set; } = null!;


        public virtual ICollection<Product> Products { get; set; }
    }
}
