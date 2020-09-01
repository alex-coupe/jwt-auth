using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
