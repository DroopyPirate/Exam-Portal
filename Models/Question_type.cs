using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Question_type
    {
        [Key]
        public int id { get; set; }

        [Required]
        public bool question_type { get; set; }



        public ICollection<Question> Questions { get; set; }
    }
}
