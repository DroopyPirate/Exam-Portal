using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Groups
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Creator_id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Branch { get; set; }
        public int? Semester { get; set; }  //nullable
        public string Division { get; set; }



        [ForeignKey("Creator_id")]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }

        public ICollection<AssignedTest> AssignedTests { get; set; }
    }
}
