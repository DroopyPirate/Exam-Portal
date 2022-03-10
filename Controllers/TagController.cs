using Exam_Portal.Models;
using Exam_Portal.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class TagController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public TagController(UserManager<ApplicationUser> userManager,
                             AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string id = userManager.GetUserId(HttpContext.User);
            var user = await userManager.FindByIdAsync(id);
            var tags = (from t in context.Tags where t.Creator_id == user.Id select t).ToList();
            foreach(var tag in tags)
            {
                tag.Questions = (from q in context.Questions where q.Tag_id == tag.Id select q).ToList();
            }

            var model = new CreateTagViewModel
            {
                Create_Tag_Name = "",
                Tags = tags,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateTag(CreateTagViewModel model)
        {
            string id = userManager.GetUserId(HttpContext.User);
            var tag = new Tag
            {
                Tag_name = model.Create_Tag_Name,
                Creator_id = Convert.ToInt32(id)
            };

            context.Tags.Add(tag);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteTag(int id)
        {
            var tag = context.Tags.Find(id);
            context.Tags.Remove(tag);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Rename(int id)
        {
            var tag = context.Tags.Find(id);
            var model = new CreateTagViewModel
            {
                Create_Tag_Name = tag.Tag_name,
                Tags = null
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Rename(int id, CreateTagViewModel model)
        {
            var tag = context.Tags.Find(id);
            tag.Tag_name = model.Create_Tag_Name;
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
