using Exam_Portal.Models;
using Exam_Portal.ViewModels.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    public class ResultController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public ResultController(UserManager<ApplicationUser> userManager,
                               AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }


        [HttpGet]
        public IActionResult Results(int id)
        {
            var model = new ResultsViewModel
            {
                Test = context.Tests.Find(id)
            };
            model.Test.TestType = context.TestTypes.Find(model.Test.Type_id);

            var currentDate = DateTime.Now;
            var endDate = Convert.ToDateTime(model.Test.EndDate);
            if (endDate > currentDate)
            {
                model.IsActive = true;
            }

            List<int> assignedGroupIds = (from at in context.AssignedTests 
                                          where at.Test_id == id
                                          select at.Group_id).ToList();
            List<ApplicationUser> users = new();

            foreach(int aG_id in assignedGroupIds)
            {
                var userIdList = (from ug in context.UserGroups
                           where ug.Group_id == aG_id
                           select ug.User_id).ToList();
                foreach(int userId in userIdList)
                {
                    var user = context.ApplicationUser.Find(userId);
                    var userResult = new UserResult();
                    userResult.FullName = user.Name + " " + user.MiddleName + " " + user.LastName;
                    userResult.Division = user.Division;
                    userResult.Semester = user.Semester;

                    var totalResult = (from tr in context.TotalResults
                                       where tr.Test_id == id && tr.User_id == user.Id
                                       select tr).ToList();
                    if(totalResult.Count == 0)
                    {
                        userResult.GivenTest = false;
                    }
                    else
                    {
                        userResult.GivenTest = true;
                        userResult.Result = totalResult[0].Result == true ? "Pass" : "Fail";
                        userResult.Marks = totalResult[0].Marks_obtained + "/" + totalResult[0].Total_marks;
                    }
                    model.UserResults.Add(userResult);
                }
            }

            return View(model);
        }

    }
}
