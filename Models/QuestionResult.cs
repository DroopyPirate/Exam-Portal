using System;
using System.Collections.Generic;
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
    }
}
