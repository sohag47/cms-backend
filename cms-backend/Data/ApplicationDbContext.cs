using cms_backend.Helpers;
using cms_backend.Models;
using cms_backend.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace cms_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply soft-delete filter for all AuditableEntity models
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(GetSoftDeleteFilter(entityType.ClrType));
                }
            }

            // 👇 Seed users
            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = 1,
            //        Username = "admin",
            //        Email = "admin@example.com",
            //        PasswordHash = "12345678",
            //        CreatedAt = new DateTime(2024, 01, 01, 10, 0, 0, DateTimeKind.Utc),
            //        CreatedBy = 1
            //    },
            //    new User
            //    {
            //        Id = 2,
            //        Username = "john",
            //        Email = "john@example.com",
            //        PasswordHash = "12345678",
            //        CreatedAt = new DateTime(2024, 01, 01, 10, 0, 0, DateTimeKind.Utc),
            //        CreatedBy = 1
            //    }
            //);

            // 👇 Seed posts
            //modelBuilder.Entity<Post>().HasData(
            //    new Post
            //    {
            //        Id = 1,
            //        Title = "Welcome to the Blog",
            //        Slug = "welcome-blog",
            //        Content = "This is the first blog post.",
            //        AuthorId = 1,
            //        CreatedAt = new DateTime(2024, 01, 01, 10, 0, 0, DateTimeKind.Utc),
            //        CreatedBy = 1
            //    },
            //    new Post
            //    {
            //        Id = 2,
            //        Title = "EF Core Tips",
            //        Slug = "ef-core-tips",
            //        Content = "Learn how to use EF Core effectively.",
            //        AuthorId = 2,
            //        CreatedAt = new DateTime(2024, 01, 01, 10, 0, 0, DateTimeKind.Utc),
            //        CreatedBy = 1
            //    }
            //);
        }

        private static LambdaExpression GetSoftDeleteFilter(Type entityType)
        {
            var param = Expression.Parameter(entityType, "e");
            var prop = Expression.Property(param, nameof(AuditableEntity.DeletedAt));
            var body = Expression.Equal(prop, Expression.Constant(null));
            return Expression.Lambda(body, param);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = 1;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = 1;
                        break;

                    case EntityState.Deleted:
                        // Soft delete
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedAt = now;
                        entry.Entity.DeletedBy = 1;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
