using System.Data.Entity.ModelConfiguration;

namespace DataLayer.Models.Mapping
{
    public class PhoneMap : EntityTypeConfiguration<Phone>
    {
        public PhoneMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNumber)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Phone");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.PhoneType)
                .WithMany(t => t.Phones)
                .HasForeignKey(d => d.Type);
            this.HasRequired(t => t.UserProfile)
                .WithMany(t => t.Phones)
                .HasForeignKey(d => d.UserId);

        }
    }
}
