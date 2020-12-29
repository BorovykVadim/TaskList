using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskList.Core.Models;

namespace TaskList.Data
{
    public class TaskListDbContext : IdentityDbContext<User, Role, int>
    {
        public TaskListDbContext(DbContextOptions<TaskListDbContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.Author)
                .HasForeignKey(u => u.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
