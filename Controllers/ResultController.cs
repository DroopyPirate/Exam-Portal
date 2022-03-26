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
                    userResult.userID = userId;
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

        [HttpGet]
        public IActionResult ViewResult(string id)
        {
            int splitIndex = id.IndexOf('|');
            int length = id.Length;
            int testID = Convert.ToInt32(id.Substring(0, splitIndex));
            int userId = Convert.ToInt32(id.Substring((splitIndex+1), (length-splitIndex-1)));

            //int testID = Convert.ToInt32(id);
            var model = new ViewResultViewModel();
            //int userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User)); //get user Id
            model.Tests = context.Tests.Find(testID);  // get Test user appeared
            model.Tests.TestType = context.TestTypes.Find(model.Tests.Type_id); //get testType of test

            var totalResult = (from tr in context.TotalResults
                               where tr.User_id == userId && tr.Test_id == testID
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

            foreach (var tq in model.Tests.TestQuestions)
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
