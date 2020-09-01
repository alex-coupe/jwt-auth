using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Data
{
    public class UserDTO
    {

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
