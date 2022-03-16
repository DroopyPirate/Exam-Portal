using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Admin
{
    public class ViewTestViewModel
    {
        public ViewTestViewModel()
        {
            TestExtendeds = new List<TestExtended>();
        }

        public List<TestExtended> TestExtendeds { get; set; }
    }

    public class TestExtended : Test
    {
        public int? NoOfQuestions { get; set; }
        public string CreatorName { get; set; }
        public string Type_name { get; set; }
    } 
}
