using Exam_Portal.Models;
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
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PassingMarks = model.PassingMarks,
                Type_id = type_ids[0],
                Faculty_id = user_id
            };

            context.Tests.Add(test);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TestDetails(int id)
        {
            var testList = (from t in context.Tests
                            where t.Id == id select t).ToList();
            var test = testList[0];
            test.TestType = (from tt in context.TestTypes 
                             where tt.Id == test.Type_id select tt).ToList()[0];
            //converted to list and appended first value as only one value exists
            int count = (from tq in context.TestQuestions
                         where tq.Test_id == test.Id
                         select tq).ToList().Count();

            var model = new TestDetailsViewModel
            {
                Test = test,
                NoOfQuestions = count,
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult AddQuestions()
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var model = new AddQuestionsViewModel();
            model.Tags = (from t in context.Tags
                          where t.Creator_id == user_id
                          select t).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddQuestions(AddQuestionsViewModel model)
        {
            return View();
        }
    }
}
