using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Storev3.Models
{
    [Authorize(Roles = "admin")]
    [Table("Pointofissue")]
    public partial class Pointofissue
    {
        public Pointofissue()
        {
            Orders = new HashSet<Order>();
        }
        [Key]
        public Guid Pointid { get; set; }
        [Required]
        public string Address { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
