using Exam_Portal.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.ViewModels
{
    public class AddFacultyViewModel
    {
        [Required]
        public RoleEnum Role { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required,MaxLength(30)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [MaxLength(30)]
        [DataType(DataType.Text)]
        public string MiddleName { get; set; }

        [Required,MaxLength(30)]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(70)]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required]
        public BranchEnum Branch { get; set; }

        //[Required]
        //public int Semester { get; set; }

        //[Required]
        //public string Division { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string DOB { get; set; }
    }
}
