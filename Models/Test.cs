using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        public int Faculty_id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Title { get; set; }

        //[Required]
        public int Type_id { get; set; }

        [Required]
        //[DataType(DataType.Time)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{hh:mm:ss}")]
        public TimeSpan? Duration { get; set; }

        [Required]
        //[DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public string StartDate { get; set; }

        [Required]
        //[DataType(DataType.Date)]
        [DisplayName("End Date")]
        public string EndDate { get; set; }

        public int? Marks { get; set; }

        [DisplayName("Passing %")]
        public int? PassingMarks { get; set; }

        //[Required]
        public string Language { get; set; } = "";



        [ForeignKey("Faculty_id")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Type_id")]
        public TestType TestType { get; set; }

        //public ICollection<Question> Questions { get; set; }
        public ICollection<TestQuestion> TestQuestions { get; set; }

        public ICollection<AssignedTest> AssignedTests { get; set; }

        public ICollection<TotalResult> TotalResults { get; set; }
    }
}
