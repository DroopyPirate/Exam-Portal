using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Group_id { get; set; }

        [Required]
        public int User_id { get; set; }



        [ForeignKey("User_id")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Group_id")]
        public Group Group { get; set; }
    }
}
