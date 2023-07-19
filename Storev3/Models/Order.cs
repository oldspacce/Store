using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Storev3.Models
{
    [Authorize(Roles = "admin, customer")]
    [Table("Order")]
    public partial class Order
    {
        public Order()
        {
            Productorders = new HashSet<Productorder>();
        }
        [Key]
        public Guid Orderid { get; set; }
        [Required]
        public string Status { get; set; } = null!;
        [ForeignKey("Userid")]
        [Required]
        public Guid Userid { get; set; }
        [ForeignKey("Pointid")]
        public Guid? Pointid { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateOnly Datedelivery { get; set; }
        [Required]
        public decimal Fullcost { get; set; }

        public virtual Pointofissue Point { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Productorder> Productorders { get; set; }

    }
}
