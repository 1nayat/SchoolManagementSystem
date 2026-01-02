using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations
{
    public class StudentGuardianConfig : IEntityTypeConfiguration<StudentGuardian>
    {
        public void Configure(EntityTypeBuilder<StudentGuardian> builder)
        {
            builder.HasKey(x => new { x.StudentId, x.GuardianId });

            builder.HasOne(x => x.Student)
                   .WithMany(x => x.Guardians)
                   .HasForeignKey(x => x.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Guardian)
                   .WithMany()
                   .HasForeignKey(x => x.GuardianId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
