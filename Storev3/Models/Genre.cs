using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Storev3.Models
{
    [Table("Genre")]
    public partial class Genre
    {
        public Genre()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public Guid Genreid { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
