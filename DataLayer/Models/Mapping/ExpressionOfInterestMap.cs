using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLayer.Models.Mapping
{
    public class ExpressionOfInterestMap : EntityTypeConfiguration<ExpressionOfInterest>
    {
        public ExpressionOfInterestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsFixedLength()
                .HasMaxLength(1500);

            // Table & Column Mappings
            this.ToTable("ExpressionOfInterest");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Message).HasColumnName("Message");
        }
    }
}
