using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AdmissionNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(x => new { x.SchoolId, x.AdmissionNumber })
               .IsUnique();

        builder.HasMany(x => x.Guardians)
               .WithOne(x => x.Student)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}


