using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street1)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(a => a.Street2)
                .HasColumnType("varchar(250)");

            builder.Property(a => a.City)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(a => a.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(a => a.PostalCode)
                .IsRequired()
                .HasColumnType("varchar(8)");

            builder.Property(a => a.Country)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Addresses");
        }
    }
}
