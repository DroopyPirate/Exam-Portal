using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Location { get; set; }

        public string Branch { get; set; }
        public int Semester { get; set; }
        public string Division { get; set; }
    }
}
