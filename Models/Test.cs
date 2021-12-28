using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int Faculty_id { get; set; }
        public string Title { get; set; }
        public bool Type { get; set; }
        public string Duration { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Marks { get; set; }
        public int PassingMarks { get; set; }
        public string Language { get; set; }

        [ForeignKey("Faculty_id")]
        public User User { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<AssignedTest> AssignedTests { get; set; }

        public ICollection<TotalResult> TotalResults { get; set; }
    }
}
