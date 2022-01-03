using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class DescriptiveAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Question_id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Answer { get; set; }



        [ForeignKey("Question_id")]
        public Question Question { get; set; }
    }
}
