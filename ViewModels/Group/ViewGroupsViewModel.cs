using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Group
{
    public class ViewGroupsViewModel
    {
        public ViewGroupsViewModel()
        {
            GroupExtendeds = new List<GroupExtended>();
        }

        public List<GroupExtended> GroupExtendeds { get; set; }
        // To take dates while assigning Test to Groups
        public string StartDate { get; set; } = "-";
        public string EndDate { get; set; } = "-";
    }

    public class GroupExtended: Groups
    {
        public int NumberOfStudents { get; set; }
        public string CreatorName { get; set; }
        public int StudentCount { get; set; }
    }
}
