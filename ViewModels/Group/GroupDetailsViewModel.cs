using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Group
{
    public class GroupDetailsViewModel
    {
        public GroupDetailsViewModel()
        {
            Users = new List<ApplicationUser>();
        }

        public List<ApplicationUser> Users { get; set; }

        public Groups Group { get; set; }
    }
}
