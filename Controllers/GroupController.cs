using Exam_Portal.Enums;
using Exam_Portal.Models;
using Exam_Portal.ViewModels;
using Exam_Portal.ViewModels.Admin;
using Exam_Portal.ViewModels.Group;
//using Exam_Portal.ViewModels.GroupVM;
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

            var group = new Groups
            {
                Creator_id = Convert.ToInt32(user),
                Name = model.Name,
                Branch = model.Branch == null ? null : model.Branch.ToString(), //To store null if value is null
                Semester = model.Semester == null ? null : model.Semester.Value,
                Division = model.Division?.ToString()                           // Can be written in shorthand as follow
            };
        
            context.Groups.Add(group);
            context.SaveChanges();

            return RedirectToAction("GroupDetails", new { id = group.Id});
        }

        [HttpGet]
        public async Task<IActionResult> AddStudentToGroup()
        {
            var model = new ViewUserViewModel();

            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, "Student"))
                {
                    model.User.Add(user);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GroupDetails(int id)
        {
            var model = new GroupDetailsViewModel      //Getting the Group for Group details
            {
                Group = context.Groups.Find(id),
            };

            List<UserGroup> userGroupsList = new();            //Storing data of usergroup in list for particular group
            foreach(var usergroup in context.UserGroups)
            {
                if(usergroup.Group_id == id)
                {
                    userGroupsList.Add(usergroup);
                }
            }

            if(userGroupsList.Any())
            {
                foreach (var usergroup in userGroupsList)      //Storing user present in usergroup
                {
                    var user = await userManager.FindByIdAsync(usergroup.User_id.ToString());
                    model.Users.Add(user);
                }
            }
            else
            {
                model.Users.Clear();      //If list empty
            }
            
            return View(model);
        }
    }
}
