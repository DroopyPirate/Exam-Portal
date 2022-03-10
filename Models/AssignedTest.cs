using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class AssignedTest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Group_id { get; set; }

        [Required]
        public int Test_id { get; set; }



        [ForeignKey("Group_id")]
        public Groups Group { get; set; }

        [ForeignKey("Test_id")]
        public Test Test { get; set; }
    }
}
