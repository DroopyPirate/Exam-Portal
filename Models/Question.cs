using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int Test_id { get; set; }
        public int Tag_id { get; set; }
        public string Question_ { get; set; }
        public int Marks { get; set; }

        [ForeignKey("Test_id")]
        public Test Test { get; set; }

        [ForeignKey("Tag_id")]
        public Tag Tag { get; set; }

        public ICollection<Option> Options { get; set; }

        public ICollection<QuestionResult> QuestionResults { get; set; }
    }
}
