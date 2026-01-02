using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(x => x.Email)
               .IsUnique();

        builder.HasOne(x => x.School)
               .WithMany()
               .HasForeignKey(x => x.SchoolId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
