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
    }
}
