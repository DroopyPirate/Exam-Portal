using Exam_Portal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Group
{
    public class CreateGroupViewModel
    {
        [Required]
        public int Creator_id { get; set; }

        [Required]
        public string Name { get; set; }

        public BranchEnumNullable? Branch { get; set; }

        public DivisionEnumNullable? Division { get; set; }

        public int? Semester { get; set; }
    }
}
