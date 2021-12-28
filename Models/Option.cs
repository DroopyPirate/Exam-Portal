using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Option
    {
        public int Id { get; set; }
        public int Question_id { get; set; }
        public string Option_ { get; set; }
        public bool Is_right_choice { get; set; }

        [ForeignKey("Question_id")]
        public Question Question { get; set; }

        public ICollection<QuestionResult> QuestionResults { get; set; }
    }
}
