using System.Data.Entity;
using DataLayer.Models.Mapping;

namespace DataLayer.Models
{
    public partial class NavasthalaContext : DbContext
    {
        static NavasthalaContext()
        {
            Database.SetInitializer<NavasthalaContext>(null);
        }

        public NavasthalaContext()
            : base("Name=NavasthalaContext")
        {
        }

        public DbSet<ExpressionOfInterest> ExpressionOfInterests { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<PhoneType> PhoneTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UpcomingProject> UpcomingProjects { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ExpressionOfInterestMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new UpcomingProjectMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressTypeMap());
            modelBuilder.Configurations.Add(new PhoneMap());
            modelBuilder.Configurations.Add(new PhoneTypeMap());


        }
    }
}
