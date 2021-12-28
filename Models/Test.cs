using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int Faculty_id { get; set; }
        public string Title { get; set; }
        public bool Type { get; set; }
        public string Duration { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Marks { get; set; }
        public int PassingMarks { get; set; }
        public string Language { get; set; }
    }
}
