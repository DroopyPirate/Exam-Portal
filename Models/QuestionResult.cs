using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class QuestionResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int User_id { get; set; }

        [Required]
        public int TestQuestions_id { get; set; }

        //[Required]
        //public int Option_id { get; set; }

        [Required]
        public string User_answer { get; set; }

        public int Marks { get; set; }

        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        //public string Answer_time { get; set; }



        [ForeignKey("User_id")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("TestQuestions_id")]
        public TestQuestion TestQuestion { get; set; }
    }
}
