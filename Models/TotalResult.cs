using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("User_id")]
        public User User { get; set; }

        [ForeignKey("Test_id")]
        public Test Test { get; set; }
    }
}
