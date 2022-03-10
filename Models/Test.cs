using System;
using System.Collections.Generic;
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

        [Required]
        public int Faculty_id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Type { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public string Duration { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }

        public int Marks { get; set; }

        public int PassingMarks { get; set; }

        [Required]
        public string Language { get; set; }



        [ForeignKey("Faculty_id")]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<AssignedTest> AssignedTests { get; set; }

        public ICollection<TotalResult> TotalResults { get; set; }
    }
}
