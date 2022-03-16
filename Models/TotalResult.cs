using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class TotalResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int User_id { get; set; }

        [Required]
        public int Test_id { get; set; }

        [Required]
        public int Total_marks { get; set; }

        [Required]
        public int Marks_obtained { get; set; }

        [Required]
        public bool Result { get; set; }



        [ForeignKey("User_id")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Test_id")]
        public Test Test { get; set; }
    }
}
