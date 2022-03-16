using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Result
{
    public class ResultsViewModel
    {
        public ResultsViewModel()
        {
            UserResults = new List<UserResult>();
        }

        public Test Test { get; set; }
        public bool IsActive { get; set; }
        public List<UserResult> UserResults { get; set; }
    }

    public class UserResult
    {
        public string FullName { get; set; }
        public string Division { get; set; }
        public int Semester { get; set; }
        public string Marks { get; set; }
        public string Result { get; set; }
        public bool GivenTest { get; set; }
    }
}
