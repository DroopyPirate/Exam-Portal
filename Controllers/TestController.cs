using Exam_Portal.Models;
using Exam_Portal.ViewModels.Group;
using Exam_Portal.ViewModels.Tests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    public class TestController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public TestController(UserManager<ApplicationUser> userManager,
                             AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var model = new CreateTestViewModel
            {
                TestTypes = (from tt in context.TestTypes select tt).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateTest(CreateTestViewModel model)
        {
            var type_ids = (from tt in context.TestTypes 
                           where tt.Type_name == model.Type_name 
                           select tt.Id).ToList();
            
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var test = new Test
            {
                Title = model.Title,
                Duration = model.Duration,
                StartDate = "-",
                EndDate = "-",
                PassingMarks = model.PassingMarks,
                Type_id = type_ids[0],
                Faculty_id = user_id
            };

            context.Tests.Add(test);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> TestDetails(int id)
        {
            var test = context.Tests.Find(id);

            //Check if test is created by LoggedIn User
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            bool createdByUser = (test.Faculty_id == user_id); //Check if test created by current user

            test.TestType = context.TestTypes.Find(test.Type_id);
            //converted to list and appended first value as only one value exists
            int count = (from tq in context.TestQuestions
                         where tq.Test_id == test.Id
                         select tq).ToList().Count();

            var model = new TestDetailsViewModel
            {
                Test = test,
                NoOfQuestions = count,
                byUser = createdByUser
            };

            //For Questions in this Test

            var testQuestionsId = (from tq in context.TestQuestions
                                   where tq.Test_id == id
                                   select tq.Question_id).ToList(); //Questions of this test
            foreach(int q_id in testQuestionsId)
            {
                var question = await context.Questions.FindAsync(q_id);
                question.Tag = await context.Tags.FindAsync(question.Tag_id);
                model.Questions.Add(question);
            }

            //For Groups that are assigned this Test

            var assignedTestGroups = (from at in context.AssignedTests
                                      where at.Test_id == id 
                                      select at.Group_id).ToList(); //Group ids assigned to this Test
            var groups = (from g in context.Groups 
                          where assignedTestGroups.Contains(g.Id) 
                          select g).ToList(); //All groups
            foreach (var group in groups)
            {
                var group_id = group.Id;                         // Get id of the group
                var studentcount = (from b in context.UserGroups 
                                    where b.Group_id == group_id 
                                    select b).Count();   // Get the count of the entries    

                var faculty = await userManager.FindByIdAsync(group.Creator_id.ToString());  //Get faculty by creator_id to get faculty name

                var modelgroup = new GroupExtended
                {
                    Id = group.Id,
                    Name = group.Name,
                    CreatorName = faculty.Name + " " + faculty.LastName,
                    StudentCount = studentcount,
                    Branch = group.Branch,
                    Semester = group.Semester,
                    Division = group.Division,
                };
                model.GroupExtendeds.Add(modelgroup);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddQuestions(int id)
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var model = new AddQuestionsViewModel();
            model.Tags = (from t in context.Tags
                          where t.Creator_id == user_id
                          select t).ToList();
            model.Test_id = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestions(AddQuestionsViewModel model)
        {
            int iterator = model.Question.Length;
            var test = await context.Tests.FindAsync(model.Test_id);
            int totalMarks = Convert.ToInt32(test.Marks);
            for(int i=0; i<iterator; i++)
            {
                var question = new Question();

                question.Tag_id = model.Tag_id[i];
                question.Question_ = model.Question[i];
                question.Option1 = model.Option1[i];
                question.Option2 = model.Option2[i];
                question.Option3 = model.Option3[i];
                question.Option4 = model.Option4[i];
                //question.Answer = model.Answer[i];
                question.Marks = model.Marks[i];
                question.Description = model.Description[i];
                switch (model.Answer[i])
                {
                    case "0":
                        question.Answer = question.Option1;
                        break;
                    case "1":
                        question.Answer = question.Option2;
                        break;
                    case "2":
                        question.Answer = question.Option3;
                        break;
                    case "3":
                        question.Answer = question.Option4;
                        break;
                    default:
                        question.Answer = "Switch Case default";
                        break;
                }
                
                totalMarks += question.Marks;
                context.Questions.Add(question);
                context.SaveChanges();

                var testQuestion = new TestQuestion
                {
                    Test_id = model.Test_id,
                    Question_id = question.Id
                };
                context.TestQuestions.Add(testQuestion);
                context.SaveChanges();
            }
            test.Marks = totalMarks;
            context.Tests.Update(test);
            context.SaveChanges();

            return RedirectToAction("TestDetails", new { id = model.Test_id});
        }

        [HttpGet]
        public async Task<IActionResult> AssignTest(int id)
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var assignedGroups = (from at in context.AssignedTests 
                                  where at.Test_id == id 
                                  select at.Group_id).ToList(); //Groups assigned to this test
            var groups = (from g in context.Groups 
                          where !assignedGroups.Contains(g.Id) 
                          select g).ToList();  //Get all groups and avoid groups already assigned
            var model = new ViewGroupsViewModel();

            foreach (var group in groups)
            {
                var group_id = group.Id;                         // Get id of the group
                var studentcount = from b in context.UserGroups where b.Group_id == group_id select b;   //Get all entries of usergroup for that group id
                var count = studentcount.Count();    // Get the count of the entries
                if (count == 0) { continue; } //If a group has no students then dont display that group

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
            //To take startdate and enddate in model to see if dates are yet allocated or not.
            var test = context.Tests.Find(id);
            model.StartDate = test.StartDate;
            model.EndDate = test.EndDate;

            return View(model);
        }

        [HttpPost]
        public IActionResult AssignTest([FromBody] string[] array)
        {
            int test_id = Convert.ToInt32(array[array.Length - 1]);
            string start_Date = array[array.Length - 2];
            start_Date = start_Date.Replace('T', ' ');
            string end_Date = array[array.Length - 3];
            end_Date = end_Date.Replace('T', ' ');
            array = array.Take(array.Length - 3).ToArray();

            var test = context.Tests.Find(test_id);
            test.StartDate = start_Date;
            test.EndDate = end_Date;
            test.isActive = true;
            context.SaveChanges();

            foreach(var a in array)
            {
                int group_id = Convert.ToInt32(a);
                var assignedTest = new AssignedTest
                {
                    Group_id = group_id,
                    Test_id = test_id,
                };
                context.AssignedTests.Add(assignedTest);
                context.SaveChanges();
            }

            return Json(new { status = true });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = context.Tests.Find(id);

            context.Remove(test);
            context.SaveChanges();

            var user = await userManager.GetUserAsync(HttpContext.User);
            var roleList = await userManager.GetRolesAsync(user);

            if(roleList.Contains("Faculty"))
            {
                return RedirectToAction("ViewTest", "Faculty");
            }
            else
            {
                return RedirectToAction("ViewTest", "Admin");
            }       
        }
    }
}
