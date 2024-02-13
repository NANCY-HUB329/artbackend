
    using Authentications.Models;
using Authentications.Models.AuthService.Models;
using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
namespace Authentications.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        }
    }

