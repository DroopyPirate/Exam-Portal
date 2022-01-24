using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Group
{
    public class ExtendApplicationUser: ApplicationUser
    {
        public bool isChecked { get; set; }
    }

    public class AddStudentToGroupViewModel
    {
        public AddStudentToGroupViewModel()
        {
            User = new List<ExtendApplicationUser>();
        }

        public List<ExtendApplicationUser> User { get; set; }

        public int[] SelectedUser { get; set; }
    }
}
