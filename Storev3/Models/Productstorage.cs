using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Storev3.Models
{
    [Authorize(Roles = "admin")]
    [Table("Productstorage")]
    public partial class Productstorage
    {
        [Key]
        public Guid Productstorageid { get; set; }
        [ForeignKey("Productid")]
        [Required]
        public Guid Productid { get; set; }
        [ForeignKey("Storeageid")]
        [Required]
        public Guid Storageid { get; set; }
        [Required]
        public int Count { get; set; }


        public virtual Product Product { get; set; } = null!;
        public virtual Storage Storage { get; set; } = null!;
    }
}
