using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class TotalResult
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Test_id { get; set; }
        public int Total_marks { get; set; }
        public bool Result { get; set; }
    }
}
