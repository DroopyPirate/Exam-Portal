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
        public int Question_id { get; set; }

        [Required]
        public int Option_id { get; set; }

        public bool Is_right { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public string Answer_time { get; set; }



        [ForeignKey("User_id")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Question_id")]
        public Question Question { get; set; }

        [ForeignKey("Option_id")]
        public Option Option { get; set; }
    }
}
