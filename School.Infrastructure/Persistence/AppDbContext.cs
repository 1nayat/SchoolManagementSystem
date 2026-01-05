using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Domain.Common;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<School.Domain.Entities.School> Schools => Set<School.Domain.Entities.School>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Guardian> Guardians => Set<Guardian>();
    public DbSet<StudentGuardian> StudentGuardians => Set<StudentGuardian>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<ClassSubjectTeacher> ClassSubjectTeachers => Set<ClassSubjectTeacher>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<StudentAttendance> StudentAttendances => Set<StudentAttendance>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<ExamResult> ExamResults => Set<ExamResult>();
    public DbSet<Fee> Fees => Set<Fee>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookBorrow> BookBorrows => Set<BookBorrow>();

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ApplySoftDeleteQueryFilter(modelBuilder);
    }

    private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(SoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
                continue;

          
            var parameter = Expression.Parameter(entityType.ClrType, "e");

            var propertyMethod = typeof(EF)
                .GetMethod(nameof(EF.Property), new[] { typeof(object), typeof(string) })!
                .MakeGenericMethod(typeof(bool));

            var isDeletedProperty = Expression.Call(
                propertyMethod,
                parameter,
                Expression.Constant(nameof(SoftDeleteEntity.IsDeleted))
            );

            var compareExpression = Expression.Equal(
                isDeletedProperty,
                Expression.Constant(false)
            );

            var lambda = Expression.Lambda(compareExpression, parameter);

            modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(lambda);
        }
    }
}
