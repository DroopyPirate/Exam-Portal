using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Tests
{
    public class CreateTestViewModel
    {
        //[Key]
        //public int Id { get; set; }

        //public int Faculty_id { get; set; }

        [Required]
        [DisplayName("Test Name")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Test Type")]
        public string Type_name { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{hh:mm:ss}")]
        public TimeSpan? Duration { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        public string StartDate { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)] 
        public string EndDate { get; set; }

        //public int? Marks { get; set; }

        [Required]
        [DisplayName("Passing Percentage")]
        public int? PassingMarks { get; set; }

        public List<TestType> TestTypes { get; set; }

        //[Required]
        //public string Language { get; set; } = "";
    }
}
