using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Infrastructure.Data
{
    public class BloggerContext : DbContext
    {
        public BloggerContext(DbContextOptions options) : base(options)
        {         
        }
        public DbSet<Post> Posts { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AudiTableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AudiTableEntity)entityEntry.Entity).LastModified = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((AudiTableEntity)entityEntry.Entity).Created = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }
    }
}
