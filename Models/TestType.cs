using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class TestType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Type")]
        public string Type_name { get; set; }

        public List<Test> Tests { get; set; }
    }
}
