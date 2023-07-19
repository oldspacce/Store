using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Storev3.Models
{
    [Authorize(Roles = "admin")]
    [Table("Storage")]
    public partial class Storage
    {
        public Storage()
        {
            Productstorages = new HashSet<Productstorage>();
        }
        [Key]
        public Guid Storageid { get; set; }
        [Required]
        public string Address { get; set; } = null!;
        public virtual ICollection<Productstorage> Productstorages { get; set; }
    }
}
