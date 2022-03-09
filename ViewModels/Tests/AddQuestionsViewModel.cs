using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Tests
{
    public class AddQuestionsViewModel
    {
        public AddQuestionsViewModel()
        {
            Tags = new List<Tag>();
        }
        [Required]
        [DisplayName("Question")]
        public string[] Question { get; set; }

        [Required]
        [DisplayName("Option 1")]
        public string[] Option1 { get; set; }

        [Required]
        [DisplayName("Option 2")]
        public string[] Option2 { get; set; }

        [Required]
        [DisplayName("Option 3")]
        public string[] Option3 { get; set; }

        [Required]
        [DisplayName("Option 4")]
        public string[] Option4 { get; set; }

        [Required]
        public string[] Answer { get; set; }

        [Required]
        [DisplayName("Description")]
        public string[] Description { get; set; }

        [Required]
        [DisplayName("Marks")]
        public int[] Marks { get; set; }

        [Required]
        [DisplayName("Tag")]
        public string[] Tag_name { get; set; }

        public List<Tag> Tags { get; set; }
    }


}
