using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Storev3.Models
{
    [Authorize(Roles = "admin, customer")]
    [Table("Productorder")]
    public partial class Productorder
    {
        [Key]
        public Guid Productorderid { get; set; }
        [ForeignKey("Productid")]
        [Required]
        public Guid Productid { get; set; }
        [ForeignKey("Orderid")]
        [Required]
        public Guid Orderid { get; set; }
        [Required]
        public int Count { get; set; }


        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
