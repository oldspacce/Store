using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Storev3.Models
{
    [Authorize(Roles = "admin")]
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Userscores = new HashSet<Userscore>();
        }
        [Key]
        public Guid Userid { get; set; }
        [Required]
        public string Surname { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        [MaxLength(13)]
        [RegularExpression(@"^\+[0-9]{1,3}([0-9]{10})", ErrorMessage = "Некоректный номер")]
        public string? Phone { get; set; }
        public int? Role { get; set; }

        public virtual Role Roleid { get; set; } = null;
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Userscore> Userscores { get; set; }
    }
    public class LoginModel
    {
        public Guid Userid { get; set; }
        [Required]
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class CartModel
    {
        public IEnumerable<Product> Product1 { get; set; }
        public IEnumerable<Productorder> Productorder1 { get; set; }
        public IEnumerable<Order> Order1 { get; set; }
        public Order ConfOrder { get; set; }
    }

}
