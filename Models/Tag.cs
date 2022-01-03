using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Tag_name { get; set; }



        public ICollection<Question> Questions { get; set; }
    }
}
