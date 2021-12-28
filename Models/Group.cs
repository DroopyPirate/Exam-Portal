using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public string Branch { get; set; }
        public int Semester { get; set; }
        public string Division { get; set; }

        [ForeignKey("User_id")]
        public User User { get; set; }

        public ICollection<AssignedTest> AssignedTests { get; set; }
    }
}
