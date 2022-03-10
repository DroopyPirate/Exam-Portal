using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Test_id { get; set; }

        [Required]
        public int Question_id { get; set; }



        [ForeignKey("Test_id")]
        public Test Test { get; set; }

        [ForeignKey("Question_id")]
        public Question Question { get; set; }
    }
}
