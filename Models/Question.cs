using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Test_id { get; set; }

        [Required]
        public int Tag_id { get; set; }

        public int Question_type_id { get; set; }

        [Required]
        public string Question_ { get; set; }

        [Required]
        public int Marks { get; set; }



        [ForeignKey("Test_id")]
        public Test Test { get; set; }

        [ForeignKey("Tag_id")]
        public Tag Tag { get; set; }

        [ForeignKey("Question_type_id")]
        public Question_type Question_Type { get; set; }

        public ICollection<Option> Options { get; set; }

        public ICollection<QuestionResult> QuestionResults { get; set; }

        public ICollection<DescriptiveAnswer> DescriptiveAnswers { get; set; }

        public ICollection<DescriptiveResult> DescriptiveResults { get; set; }
    }
}
