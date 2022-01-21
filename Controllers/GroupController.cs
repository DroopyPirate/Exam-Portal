using Exam_Portal.Enums;
using Exam_Portal.Models;
using Exam_Portal.ViewModels.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class GroupController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public GroupController(UserManager<ApplicationUser> userManager,
                               AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }




        [HttpGet]
        public IActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGroup(CreateGroupViewModel model)
        {
            var user = userManager.GetUserId(HttpContext.User);

            var group = new Group
            {
                Creator_id = Convert.ToInt32(user),
                Name = model.Name,
                Branch = model.Branch == null ? null : model.Branch.ToString(), //To store null if value is null
                Semester = model.Semester == null ? null : model.Semester.Value,
                Division = model.Division?.ToString()                           // Can be written in shorthand as follow
            };

            context.Groups.Add(group);
            context.SaveChanges();

            return RedirectToAction("AddStudentToGroup");
        }

        [HttpGet]
        public IActionResult AddStudentToGroup()
        {
            return View();
        }
    }
}
