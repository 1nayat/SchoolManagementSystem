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


    private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(SoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");

            var isDeletedProperty = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                new[] { typeof(bool) },
                parameter,
                Expression.Constant(nameof(SoftDeleteEntity.IsDeleted))
            );

            var filter = Expression.Lambda(
                Expression.Equal(isDeletedProperty, Expression.Constant(false)),
                parameter
            );

            modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(filter);
        }
    }


    private void ApplyTenantQueryFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");

            var schoolIdProperty = Expression.Property(
                parameter,
                nameof(TenantEntity.SchoolId)
            );

            var isSuperAdmin = Expression.Constant(_currentUser.IsSuperAdmin);

            Expression finalExpression;

            if (_currentUser.SchoolId == null)
            {
                finalExpression = isSuperAdmin;
            }
            else
            {
                var currentSchoolId = Expression.Constant(
                    _currentUser.SchoolId.Value,
                    typeof(Guid)
                );

                var schoolMatch = Expression.Equal(
                    schoolIdProperty,
                    currentSchoolId
                );

                finalExpression = Expression.OrElse(
                    isSuperAdmin,
                    schoolMatch
                );
            }

            var lambda = Expression.Lambda(finalExpression, parameter);

            modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(lambda);
        }
    }


    public override int SaveChanges()
    {
        ApplyTenantRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTenantRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTenantRules()
    {
        if (_currentUser.IsSuperAdmin)
            return;

        var schoolId = _currentUser.SchoolId
            ?? throw new UnauthorizedAccessException("Tenant context missing");

        foreach (var entry in ChangeTracker.Entries<TenantEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SchoolId = schoolId;
            }

            if (entry.State == EntityState.Modified)
            {
                var originalSchoolId =
                    (Guid)entry.OriginalValues[nameof(TenantEntity.SchoolId)]!;

                if (originalSchoolId != schoolId)
                {
                    throw new UnauthorizedAccessException(
                        "Cross-tenant update detected");
                }

                entry.Property(e => e.SchoolId).IsModified = false;
            }
        }
    }
}
