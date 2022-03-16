using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Result
{
    public class ViewResultViewModel
    {
        public ViewResultViewModel()
        {
            StudentQuestionResults = new List<StudentQuestionResult>();
        }

        public Test Tests { get; set; }
        public TotalResult TotalResult { get; set; }
        public List<StudentQuestionResult> StudentQuestionResults { get; set; }
        public bool IsActive { get; set; }  //If test active then dont show result
        public bool TestGiven { get; set; }  // Check if user has given the test or not
    }

    public class StudentQuestionResult
    {
        [Required]
        [DisplayName("Question")]
        public string Question_ { get; set; }

        [Required]
        [DisplayName("Option 1")]
        public string Option1 { get; set; }

        [Required]
        [DisplayName("Option 2")]
        public string Option2 { get; set; }

        [Required]
        [DisplayName("Option 3")]
        public string Option3 { get; set; }

        [Required]
        [DisplayName("Option 4")]
        public string Option4 { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public int Marks { get; set; }

        [Required]
        public string User_answer { get; set; }

        [Required]
        public int Obtained_Marks { get; set; }
    }
}
