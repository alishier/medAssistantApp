using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace medAssisTantApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<medAssisTantApp.Models.Doctor> Doctor { get; set; }
        public DbSet<medAssisTantApp.Models.MedCard> MedCard { get; set; }
        public DbSet<medAssisTantApp.Models.Patient> Patient { get; set; }
    }
}
