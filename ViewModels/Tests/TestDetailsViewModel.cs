using Exam_Portal.Models;
using Exam_Portal.ViewModels.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Tests
{
    public class TestDetailsViewModel
    {
        public TestDetailsViewModel()
        {
            Questions = new List<Question>();
            GroupExtendeds = new List<GroupExtended>();
        }

        public Test Test { get; set; }
        public List<Question> Questions { get; set; }
        public int NoOfQuestions { get; set; }
        public bool byUser { get; set; }  //To check if test created by Logged In User or not
        public List<GroupExtended> GroupExtendeds { get; set; }
    }
}
