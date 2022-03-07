using Exam_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels.Tags
{
    public class CreateTagViewModel
    {
        [Required]
        [DisplayName("Tag Name")]
        public string Create_Tag_Name { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
