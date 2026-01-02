using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class ClassSubjectTeacherConfig : IEntityTypeConfiguration<ClassSubjectTeacher>
{
    public void Configure(EntityTypeBuilder<ClassSubjectTeacher> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new
        {
            x.ClassId,
            x.SectionId,
            x.SubjectId,
            x.TeacherId
        }).IsUnique();

        builder.HasOne(x => x.Class)
               .WithMany()
               .HasForeignKey(x => x.ClassId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Section)
               .WithMany()
               .HasForeignKey(x => x.SectionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Subject)
               .WithMany()
               .HasForeignKey(x => x.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Teacher)
               .WithMany()
               .HasForeignKey(x => x.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
