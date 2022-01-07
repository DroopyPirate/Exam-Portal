using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Admin
{
    public class ViewFacultyViewModel
    {
        public ViewFacultyViewModel()
        {
            User = new List<ApplicationUser>();
        }

        //public List<string> FirstName { get; set; }

        //public List<string> LastName { get; set; }

        //public List<string> Branch { get; set; }

        public List<ApplicationUser> User { get; set; }
    }
}
