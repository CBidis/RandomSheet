﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Timesheets.Domain.Models;

namespace Timesheets.Persistence
{
    /// <summary>
    /// Timesheet Unit of Work
    /// </summary>
    public class TimesheetDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>,
                                                            IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public TimesheetDbContext(DbContextOptions<TimesheetDbContext> options) : base(options)
        {
        }

        public new DbSet<User> Users { get; set; }
        public new DbSet<Role> Roles { get; set; }
        public DbSet<Department> Deparments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDepartment> ProjectsDepartments { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //one-to-one mapping
            builder.Entity<Timesheet>()
                .HasOne(user => user.User)
                .WithOne(p => p.Timesheet)
                .HasForeignKey<Timesheet>(p => p.UserId);

            // many-to-many (Deparment - Related Projects)
            builder.Entity<ProjectDepartment>()
                .HasKey(pd => pd.Id);

            builder.Entity<ProjectDepartment>()
                .HasOne(pd => pd.Project)
                .WithMany(p => p.RelatedDeparments)
                .HasForeignKey(pd => pd.ProjectId);

            builder.Entity<ProjectDepartment>()
                .HasOne(pd => pd.Department)
                .WithMany(d => d.RelatedProjects)
                .HasForeignKey(pd => pd.DepartmentId);

            // one-to-many (Owned Projects - Deparment)
            builder.Entity<Department>()
                .HasMany(d => d.OwnedProjects)
                .WithOne(p => p.OwnerDeparment)
                .HasForeignKey(p => p.OwnerDeparmentId)
                .OnDelete(DeleteBehavior.Restrict); // https://stackoverflow.com/questions/41711772/entity-framework-core-cascade-delete-one-to-many-relationship
        }
    }
}
