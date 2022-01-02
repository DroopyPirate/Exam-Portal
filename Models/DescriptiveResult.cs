using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class DescriptiveResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int User_id { get; set; }

        [Required]
        public int Question_id { get; set; }

        [Required]
        public string User_answer { get; set; }

        public int Marks { get; set; }



        [ForeignKey("User_id")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Question_id")]
        public Question Question { get; set; }
    }
}
