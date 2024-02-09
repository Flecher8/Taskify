using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.DAL
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public override DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectIncome> ProjectIncomes { get; set;}
        public DbSet<Section> Sections { get; set; }
        public DbSet<CustomTask> CustomTasks { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProjectInvitation> ProjectInvitations { get; set;}
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyInvitation> CompanyInvitations { get; set; }
        public DbSet<CompanyMemberRole> CompanyMemberRoles { get; set;}
        public DbSet<CompanyMember> CompanyMembers {  get; set; }
        public DbSet<CompanyExpense> CompanyExpenses { get; set; }

    }
}
