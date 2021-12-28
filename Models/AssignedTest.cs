using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class AssignedTest
    {
        public int Id { get; set; }
        public int Group_id { get; set; }
        public int Test_id { get; set; }

        [ForeignKey("Group_id")]
        public Group Group { get; set; }

        [ForeignKey("Test_id")]
        public Test Test { get; set; }
    }
}
