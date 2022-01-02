using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int User_id { get; set; }

        public string Branch { get; set; }
        public int Semester { get; set; }
        public string Division { get; set; }



        [ForeignKey("User_id")]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<AssignedTest> AssignedTests { get; set; }
    }
}
