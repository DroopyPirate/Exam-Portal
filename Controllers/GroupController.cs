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
                Branch = model.Branch == null ? "None" : model.Branch.ToString(), //To store none if value is null
                Semester = model.Semester == null ? null : model.Semester.Value,
                Division = model.Division == null ? "None" : model.Division.ToString()    
            };
        
            context.Groups.Add(group);
            context.SaveChanges();

            return RedirectToAction("GroupDetails", new { id = group.Id});
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

        [HttpGet]
        public async Task<IActionResult> AddStudentToGroup(int id)
        {
            var model = new ViewUserViewModel();

            var userInGroup = (from b in context.UserGroups where b.Group_id == id select b.User_id).ToList();

            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, "Student"))
                {
                    if (!userInGroup.Contains(user.Id))
                    {
                        model.User.Add(user);
                    }                   
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddStudentToGroup([FromBody] string[] array)
        {
            int id = Convert.ToInt32(array[array.Length - 1]);
            array = array.Take(array.Length - 1).ToArray();

            foreach(var user_id in array)
            {
                var userGroup = new UserGroup
                {
                    Group_id = id,
                    User_id = Convert.ToInt32(user_id)
                };

                context.UserGroups.Add(userGroup);
                context.SaveChanges();
            }

            return Json(new { status = true });
        }

        [HttpPost]
        public IActionResult RemoveStudent([FromBody] string[] array)
        {
            int group_id = Convert.ToInt32(array[array.Length - 1]);
            array = array.Take(array.Length - 1).ToArray();

            foreach(var user_id in array)
            {
                var user_id_int = Convert.ToInt32(user_id);
                var id = (context.UserGroups.Where(b => b.User_id == user_id_int)
                         .Where(b => b.Group_id == group_id)).ToList();
                context.UserGroups.Remove(id[0]);
            }
            context.SaveChanges();

            return Json(new { status = true });
        }

        [HttpGet]
        public async Task<IActionResult> ViewGroups()
        {
            var groups = (from g in context.Groups select g).ToList();  //Get all groups
            var model = new ViewGroupsViewModel();

            foreach (var group in groups)
            {
                var id = group.Id;                         // Get id of the group
                var studentcount = from b in context.UserGroups where b.Group_id == id select b;   //Get all entries of usergroup for that group id
                var count = studentcount.Count();    // Get the count of the entries

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
            return View(model);
        }

        [HttpGet]
        public IActionResult EditGroup(int id)
        {
            var group = context.Groups.Find(id);
            var model = new CreateGroupViewModel
            {
                Name = group.Name,
                Creator_id = group.Creator_id,
                Branch = (BranchEnumNullable)Enum.Parse(typeof(BranchEnumNullable), group.Branch),
                Semester = group.Semester,
                Division = (DivisionEnumNullable)Enum.Parse(typeof(DivisionEnumNullable), group.Division)
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditGroup(int id, CreateGroupViewModel model)
        {
            var group = context.Groups.Find(id);
            group.Name = model.Name;
            group.Branch = model.Branch.ToString();
            group.Semester = model.Semester;
            group.Division = model.Division.ToString();
            context.SaveChanges();

            return RedirectToAction("GroupDetails", new { id = id});
        }

        [HttpPost]
        public IActionResult DeleteGroup(int id)
        {
            var group = context.Groups.Find(id);
            context.Groups.Remove(group);
            context.SaveChanges();

            return RedirectToAction("ViewGroups");
        }
    }
}
