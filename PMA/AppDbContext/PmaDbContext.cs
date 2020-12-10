using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PMA.AppDbContext
{
    public class PmaDbContext : IdentityDbContext<IdentityUser>
    {
        public PmaDbContext(DbContextOptions<PmaDbContext> options):
            base(options)
        {

        }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
