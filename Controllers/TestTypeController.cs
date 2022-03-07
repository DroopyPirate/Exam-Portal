using Exam_Portal.Models;
using Exam_Portal.ViewModels.TestTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    public class TestTypeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public TestTypeController(UserManager<ApplicationUser> userManager,
                             AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var testtypes = (from tt in context.TestTypes select tt).ToList();
            foreach(var testtype in testtypes)
            {
                testtype.Tests = (from t in context.Tests where t.Type_id == testtype.Id select t).ToList();
            }

            var model = new CreateTestTypeViewModel
            {
                Create_TestType_Name = "",
                TestTypes = testtypes
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateType(CreateTestTypeViewModel model)
        {
            var type = new TestType
            {
                Type_name = model.Create_TestType_Name,
            };

            context.TestTypes.Add(type);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteType(int id)
        {
            var type = context.TestTypes.Find(id);
            context.TestTypes.Remove(type);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Rename(int id)
        {
            var type = context.TestTypes.Find(id);
            var model = new CreateTestTypeViewModel
            {
                Create_TestType_Name = type.Type_name,
                TestTypes = null
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Rename(int id, CreateTestTypeViewModel model)
        {
            var type = context.TestTypes.Find(id);
            type.Type_name = model.Create_TestType_Name;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
