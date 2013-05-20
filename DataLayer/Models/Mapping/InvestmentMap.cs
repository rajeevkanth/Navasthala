using System.Data.Entity.ModelConfiguration;

namespace DataLayer.Models.Mapping
{
    public class InvestmentMap : EntityTypeConfiguration<Investment>
    {
        public InvestmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Investment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DateOfInvestment).HasColumnName("DateOfInvestment");
            this.Property(t => t.InvestedAmount).HasColumnName("InvestedAmount");
            this.Property(t => t.Maturity).HasColumnName("Maturity");
            this.Property(t => t.FinalAmount).HasColumnName("FinalAmount");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Rate).HasColumnName("Rate");

            // Relationships
            this.HasRequired(t => t.UserProfile)
                .WithMany(t => t.Investments)
                .HasForeignKey(d => d.UserId);

        }
    }
}
