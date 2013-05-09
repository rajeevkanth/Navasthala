using System.Data.Entity.ModelConfiguration;

namespace DataLayer.Models.Mapping
{
    public class PhoneTypeMap : EntityTypeConfiguration<PhoneType>
    {
        public PhoneTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Type)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("PhoneType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Type).HasColumnName("Type");
        }
    }
}
