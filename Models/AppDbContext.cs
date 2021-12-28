using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuestionResult> QuestionResults { get; set; }
        public DbSet<TotalResult> TotalResults { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<AssignedTest> AssignedTests { get; set; }
        public DbSet<Material> Materials { get; set; }
    }
}
