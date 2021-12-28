using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class User
    {
        public int Id { get; set; }
        public int Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int ContactNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
        public int Semester { get; set; }
        public string Division { get; set; }
        public string DOB { get; set; }

        public ICollection<Test> Tests { get; set; }

        public ICollection<Group> Groups { get; set; }

        public ICollection<QuestionResult> QuestionResults { get; set; }

        public ICollection<TotalResult> TotalResults { get; set; }
    }
}
