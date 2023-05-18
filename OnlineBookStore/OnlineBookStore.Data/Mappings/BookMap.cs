using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineBookStore.Core.Entities;

namespace OnlineBookStore.Data.Mappings
{
    public class BookMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(b => b.Description)
                .IsRequired();
            
            builder.Property(b => b.Cover)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(b => b.File)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .HasConstraintName("FK_Books_Authors")
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(c => c.Category)
                .WithMany(a => a.Books)
                .HasForeignKey(c => c.CategoryId)
                .HasConstraintName("FK_Books_Categories")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
