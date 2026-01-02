using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class EnrollmentConfig : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.StudentId, x.AcademicYear })
               .IsUnique();

        builder.HasOne(x => x.Student)
               .WithMany()
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Class)
               .WithMany()
               .HasForeignKey(x => x.ClassId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Section)
               .WithMany()
               .HasForeignKey(x => x.SectionId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
