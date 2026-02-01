using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Auth;
using School.Domain.Common;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly ICurrentUser _currentUser;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    // -------------------- DB SETS --------------------

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

    // -------------------- MODEL CONFIG --------------------

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ApplySoftDeleteQueryFilter(modelBuilder);
        ApplyTenantQueryFilter(modelBuilder);
    }

    // -------------------- SOFT DELETE FILTER --------------------

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

    // -------------------- TENANT (SCHOOL) FILTER --------------------

    private void ApplyTenantQueryFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");

            // e.SchoolId
            var schoolIdProperty = Expression.Property(
                parameter,
                nameof(TenantEntity.SchoolId)
            );

            // _currentUser.IsSuperAdmin
            var isSuperAdmin = Expression.Constant(_currentUser.IsSuperAdmin);

            Expression tenantExpression;

            // If SchoolId is null → only SuperAdmin can pass
            if (_currentUser.SchoolId == null)
            {
                tenantExpression = isSuperAdmin;
            }
            else
            {
                // e.SchoolId == currentUser.SchoolId (GUID vs GUID)
                var currentSchoolId = Expression.Constant(
                    _currentUser.SchoolId.Value,
                    typeof(Guid)
                );

                var schoolMatch = Expression.Equal(
                    schoolIdProperty,
                    currentSchoolId
                );

                // IsSuperAdmin || SchoolMatch
                tenantExpression = Expression.OrElse(
                    isSuperAdmin,
                    schoolMatch
                );
            }

            var lambda = Expression.Lambda(tenantExpression, parameter);

            modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(lambda);
        }
    }

}

