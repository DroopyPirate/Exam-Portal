﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        //[Key]
        //public int Id_ { get; set; }

        //[Required]
        //public int Role { get; set; }

        //[Required]
        //[DataType(DataType.Text)]
        //public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(30)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [MaxLength(30)]
        [DataType(DataType.Text)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(30)]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        //[Required]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Not a valid phone number")]
        //public int ContactNo { get; set; }

        [Required, MaxLength(70)]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        //[Required]
        //[DataType(DataType.EmailAddress)]
        //[EmailAddress(ErrorMessage = "Invalid Email format")]
        //public string Email { get; set; }

        //[Required]
        public string Branch { get; set; }

        //[Required]
        public int Semester { get; set; }

        //[Required]
        public string Division { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string DOB { get; set; }

        public bool InitialLogin { get; set; } = true;



        public List<Tag> Tags { get; set; }

        public ICollection<Test> Tests { get; set; }

        public ICollection<Groups> Groups { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }

        public ICollection<QuestionResult> QuestionResults { get; set; }

        public ICollection<DescriptiveResult> DescriptiveResults { get; set; }

        public ICollection<TotalResult> TotalResults { get; set; }
    }
}
