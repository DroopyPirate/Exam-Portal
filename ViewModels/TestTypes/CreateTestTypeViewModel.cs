using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.TestTypes
{
    public class CreateTestTypeViewModel
    {
        [Required]
        [DisplayName("Test Type Name")]
        public string Create_TestType_Name { get; set; }

        public List<TestType> TestTypes { get; set; }
    }
}
