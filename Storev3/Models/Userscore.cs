using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;


namespace Storev3.Models
{
    [Authorize(Roles = "admin, customer")]
    [Table("Userscore")]
    public class Userscore
    {
        [Required]
        public Guid Userscoreid { get; set; }
        [Required]
        public Guid Userid { get; set; }
        [Required]
        public Guid Productid { get; set; }
        [Range(0, 5, ErrorMessage = "Оценка от 1 до 5")]
        public short? Score { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
