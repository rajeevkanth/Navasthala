using System.Data.Entity.ModelConfiguration;

namespace DataLayer.Models.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AddressLineOne)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.AddressLineTwo)
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.Suburb)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.State)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.City)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(30);

            this.Property(t => t.Country)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Address");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AddressLineOne).HasColumnName("AddressLineOne");
            this.Property(t => t.AddressLineTwo).HasColumnName("AddressLineTwo");
            this.Property(t => t.Suburb).HasColumnName("Suburb");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.AddressType)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.Type);
            this.HasRequired(t => t.UserProfile)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.UserId);

        }
    }
}
