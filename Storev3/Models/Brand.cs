using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Storev3.Models
{
    [Table("Brand")]
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public Guid Brandid { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
