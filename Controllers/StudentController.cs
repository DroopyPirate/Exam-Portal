using Exam_Portal.Models;
using Exam_Portal.Repository;
using Exam_Portal.ViewModels;
using Exam_Portal.ViewModels.Admin;
using Exam_Portal.ViewModels.Group;
using Exam_Portal.ViewModels.Result;
using Exam_Portal.ViewModels.Tests;
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
    [Authorize(Roles = "Student")]
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


        [HttpGet]
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

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> ViewTest()
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            var userGroupIdList = (from ug in context.UserGroups
                                   where ug.User_id == user_id 
                                   select ug.Group_id).ToList(); //Get all ids of group user is in

            var userTestIdList = (from at in context.AssignedTests
                                  where userGroupIdList.Contains(at.Group_id) 
                                  select at.Test_id).ToList();    // Get Id of all tests assigned to groups user is in

            var userTests = (from t in context.Tests
                             where userTestIdList.Contains(t.Id) && t.isActive == true
                             select t).ToList();   //Get all user tests

            var givenTestIdList = (from tr in context.TotalResults
                                   where userTestIdList.Contains(tr.Test_id) && tr.User_id == user_id
                                   select tr.Test_id).ToList();  //Get all given tests Id

            List<int> inActiveList = new();

            var currentDate = DateTime.Now;
            foreach (var t in userTests)
            {
                if (t.EndDate != "-")
                {
                    var endDate = Convert.ToDateTime(t.EndDate);
                    if (currentDate > endDate)
                    {
                        t.isActive = false;
                        context.SaveChanges();
                        inActiveList.Add(userTests.IndexOf(t));
                    }
                }
            }

            foreach (int i in inActiveList)
            {
                userTests.RemoveAt(i);
            }

            foreach(int i in givenTestIdList)
            {
                if (userTestIdList.Contains(i))
                {
                    var test = context.Tests.Find(i);
                    userTests.Remove(test);
                }
            }

            var model = new ViewTestViewModel();

            foreach (var test in userTests)
            {
                var creator = await userManager.FindByIdAsync(test.Faculty_id.ToString());
                int count = (from tq in context.TestQuestions
                             where tq.Test_id == test.Id
                             select tq).ToList().Count;

                var modelTest = new TestExtended
                {
                    Id = test.Id,
                    Title = test.Title,
                    CreatorName = creator.Name + " " + creator.LastName,
                    NoOfQuestions = count
                };

                model.TestExtendeds.Add(modelTest);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewGivenTests()
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            var userGroupIdList = (from ug in context.UserGroups
                                   where ug.User_id == user_id
                                   select ug.Group_id).ToList(); //Get all ids of group user is in

            var userTestIdList = (from at in context.AssignedTests
                                  where userGroupIdList.Contains(at.Group_id)
                                  select at.Test_id).ToList();    // Get Id of all tests assigned to groups user is in

            var userTests = (from t in context.Tests
                             where userTestIdList.Contains(t.Id) && t.isActive == true
                             select t).ToList();   //Get all user tests

            var currentDate = DateTime.Now;
            foreach (var t in userTests)
            {
                if (t.EndDate != "-")
                {
                    var endDate = Convert.ToDateTime(t.EndDate);
                    if (currentDate > endDate)
                    {
                        t.isActive = false;
                        context.SaveChanges();
                    }
                }
            }

            var usersInActiveTests = (from t in context.Tests
                                      where userTestIdList.Contains(t.Id) && t.isActive == false
                                      select t).ToList(); //Get all inactive tests of user

            var givenTestIdList = (from tr in context.TotalResults
                                         where userTestIdList.Contains(tr.Test_id) && tr.User_id == user_id
                                         select tr.Test_id).ToList();
            foreach(int i in givenTestIdList)
            {
                var test = (from t in context.Tests
                            where t.Id == i && t.isActive == true
                            select t).ToList();
                if(test.Count != 0)
                {
                    usersInActiveTests.Add(test[0]);
                }                
            }

            var model = new ViewTestViewModel();

            foreach (var test in usersInActiveTests)
            {
                var creator = await userManager.FindByIdAsync(test.Faculty_id.ToString());
                int count = (from tq in context.TestQuestions
                             where tq.Test_id == test.Id
                             select tq).ToList().Count;

                var modelTest = new TestExtended
                {
                    Id = test.Id,
                    Title = test.Title,
                    CreatorName = creator.Name + " " + creator.LastName,
                    NoOfQuestions = count
                };

                model.TestExtendeds.Add(modelTest);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult TestDetails(int id)
        {
            var test = context.Tests.Find(id);

            test.TestType = context.TestTypes.Find(test.Type_id);
            //converted to list and appended first value as only one value exists
            int count = (from tq in context.TestQuestions
                         where tq.Test_id == test.Id
                         select tq).ToList().Count;

            var model = new TestDetailsViewModel
            {
                Test = test,
                NoOfQuestions = count,
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GiveTest(int id)
        {
            var test = context.Tests.Find(id);
            var model = new TestDetailsViewModel
            {
                Test = test
            };

            //For Questions in this Test

            var testQuestionsId = (from tq in context.TestQuestions
                                   where tq.Test_id == id
                                   select tq.Question_id).ToList(); //Questions of this test
            foreach (int q_id in testQuestionsId)
            {
                var question = await context.Questions.FindAsync(q_id);
                model.Questions.Add(question);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult GiveTest(TestDetailsViewModel model)
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var test = (from t in context.Tests where t.Id == model.Test.Id select t).ToList()[0];
            int marks = 0;
            foreach(var q in model.Questions)
            {
                var questionResult = new QuestionResult();
                int test_question_id = (from tq in context.TestQuestions
                                        where tq.Question_id == q.Id && tq.Test_id == test.Id
                                        select tq.Id).ToList()[0];
                var question = context.Questions.Find(q.Id);
                q.Answer = q.Answer switch  // simplified switch case
                {
                    "0" => question.Option1,
                    "1" => question.Option2,
                    "2" => question.Option3,
                    "3" => question.Option4,
                    _ => "",
                };
                questionResult.User_answer = q.Answer;
                questionResult.Marks = (q.Answer == question.Answer) ? question.Marks : 0;
                questionResult.TestQuestions_id = test_question_id;
                questionResult.User_id = user_id;
                marks += questionResult.Marks;
                context.QuestionResults.Add(questionResult);
                context.SaveChanges();
            }

            var totalResult = new TotalResult
            {
                User_id = user_id,
                Test_id = test.Id,
                Marks_obtained = marks,
                Total_marks = Convert.ToInt32(test.Marks)
            };         

            if((float)totalResult.Marks_obtained >= ((float)test.Marks * ((float)test.PassingMarks / 100)))
            {
                totalResult.Result = true;
            }
            else
            {
                totalResult.Result = false;
            }
            context.TotalResults.Add(totalResult);
            context.SaveChanges();

            return RedirectToAction("ViewTest", "Student");
        }

        [HttpGet]
        public IActionResult ViewResult(int id)
        {
            var model = new ViewResultViewModel();
            int userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User)); //get user Id
            model.Tests = context.Tests.Find(id);  // get Test user appeared
            model.Tests.TestType = context.TestTypes.Find(model.Tests.Type_id); //get testType of test

            var currentDate = DateTime.Now;
            var endDate = Convert.ToDateTime(model.Tests.EndDate);
            if (endDate > currentDate)
            {
                model.IsActive = true; // The test is active, hence dont show result
                return View(model);
            }
            else { model.IsActive = false; }

            var totalResult = (from tr in context.TotalResults
                               where tr.User_id == userId && tr.Test_id == id
                               select tr).ToList();  // get result of test that user appeared.
            if (totalResult.Count == 0)  // check if user has given the test
            {
                model.TestGiven = false;  // Test not given yet
                return View(model);
            }
            else
            {
                model.TestGiven = true;   // Test given
                model.TotalResult = totalResult[0];
            }

            model.Tests.TestQuestions = (from tq in context.TestQuestions
                                         where tq.Test_id == model.Tests.Id
                             
                                         select tq).ToList(); // get all testQuestion so we can get all Questions of test

            foreach(var tq in model.Tests.TestQuestions)
            {               
                var question = context.Questions.Find(tq.Question_id); // get all questions of test
                var questionResult = (from qr in context.QuestionResults
                                      where qr.User_id == userId && qr.TestQuestions_id == tq.Id
                                      select qr).ToList()[0]; //get questionResult if it matches userId and testQuestion_id
                var stdQuestionResult = new StudentQuestionResult
                {
                    Question_ = question.Question_,
                    Option1 = question.Option1,
                    Option2 = question.Option2,
                    Option3 = question.Option3,
                    Option4 = question.Option4,
                    Answer = question.Answer,
                    User_answer = questionResult.User_answer,
                    Marks = question.Marks,
                    Obtained_Marks = questionResult.Marks
                };
                model.StudentQuestionResults.Add(stdQuestionResult);
            }

            return View(model);
        }
    }
}
