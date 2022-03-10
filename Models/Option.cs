using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Option
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Question_id { get; set; }

        [Required]
        public string Option_ { get; set; }

        [Required]
        public bool Is_right_choice { get; set; }



        [ForeignKey("Question_id")]
        public Question Question { get; set; }

        //public ICollection<QuestionResult> QuestionResults { get; set; }
    }
}
