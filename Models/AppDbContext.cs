using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuestionResult> QuestionResults { get; set; }
        public DbSet<TotalResult> TotalResults { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<AssignedTest> AssignedTests { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Question_type> Question_Types { get; set; }
        public DbSet<DescriptiveAnswer> DescriptiveAnswers { get; set; }
        public DbSet<DescriptiveResult> DescriptiveResults { get; set; }
        
    }
}
