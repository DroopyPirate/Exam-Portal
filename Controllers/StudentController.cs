using Exam_Portal.Models;
using Exam_Portal.Repository;
using Exam_Portal.ViewModels;
using Exam_Portal.ViewModels.Admin;
using Exam_Portal.ViewModels.Group;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public StudentController(UserManager<ApplicationUser> userManager,
                               AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }


        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Groups()
        {
            var groups = (from g in context.Groups select g).ToList();  //Get all groups
            var model = new ViewGroupsViewModel();

            foreach (var group in groups)
            {
                var id = group.Id;                         // Get id of the group
                var userGroups = (from b in context.UserGroups where b.Group_id == id select b.User_id).ToList();   //Get all entries of usergroup for that group id
                var user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));   //Looged-in User Id

                if (userGroups.Contains(user_id)) 
                {
                    var count = userGroups.Count();    // Get the count of the entries

                    var faculty = await userManager.FindByIdAsync(group.Creator_id.ToString());  //Get faculty by creator_id to get faculty name

                    var modelgroup = new GroupExtended
                    {
                        Id = group.Id,
                        Name = group.Name,
                        CreatorName = faculty.Name + " " + faculty.LastName,
                        StudentCount = count,
                        Branch = group.Branch,
                        Semester = group.Semester,
                        Division = group.Division,
                    };
                    model.GroupExtendeds.Add(modelgroup);
                }    
            }
            return View(model);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ViewGroup(int id)
        {
            var model = new GroupDetailsViewModel      //Getting the Group for Group details
            {
                Group = context.Groups.Find(id),
            };

            List<UserGroup> userGroupsList = new();            //Storing data of usergroup in list for particular group
            foreach (var usergroup in context.UserGroups)
            {
                if (usergroup.Group_id == id)
                {
                    userGroupsList.Add(usergroup);
                }
            }

            if (userGroupsList.Any())
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
