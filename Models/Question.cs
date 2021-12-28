using System;
using System.Collections.Generic;
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
    }
}
