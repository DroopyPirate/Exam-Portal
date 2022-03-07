using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Creator_id { get; set; }

        [Required]
        public string Tag_name { get; set; }



        [ForeignKey("Creator_id")]
        public ApplicationUser ApplicationUser { get; set; }

        public List<Question> Questions { get; set; }
    }
}
