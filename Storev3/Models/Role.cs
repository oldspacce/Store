using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Storev3.Models
{
    [Authorize(Roles = "admin")]
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null;

        public virtual ICollection<User> Users { get; set; }
    }
}
