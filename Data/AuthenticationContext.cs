using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
