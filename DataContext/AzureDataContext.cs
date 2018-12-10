using Microsoft.EntityFrameworkCore;


namespace POC01.Model.DataContext
{
    public class AzureDataContext : DbContext
    {
        public AzureDataContext(DbContextOptions<AzureDataContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Dentist> Dentists { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}