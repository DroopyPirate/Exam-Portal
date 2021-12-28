using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class QuestionResult
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Question_id { get; set; }
        public int Option_id { get; set; }
        public bool Is_right { get; set; }
        public string Answer_time { get; set; }

        [ForeignKey("User_id")]
        public User User { get; set; }

        [ForeignKey("Question_id")]
        public Question Question { get; set; }

        [ForeignKey("Option_id")]
        public Option Option { get; set; }
    }
}
