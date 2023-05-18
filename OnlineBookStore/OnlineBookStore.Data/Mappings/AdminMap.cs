using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineBookStore.Core.Entities;

namespace OnlineBookStore.Data.Mappings
{
    public class AdminMap : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admin");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(a => a.Password)
                .IsRequired();
        }
    }
}
