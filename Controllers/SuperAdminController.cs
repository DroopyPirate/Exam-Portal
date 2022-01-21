using Exam_Portal.Models;
using Exam_Portal.Repository;
using Exam_Portal.ViewModels;
using Exam_Portal.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    [Authorize]
    public class SuperAdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        private readonly IFacultyRepository facultyRepository;

        public SuperAdminController(IFacultyRepository facultyRepository,
                               UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.facultyRepository = facultyRepository;
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }




        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(AddFacultyViewModel model)
        {
            List<SendEmailViewModel> email_list = new();

            if (ModelState.IsValid)
            {
                var result = await facultyRepository.CreateFaculty(model);  //Used FacultyRepo to create Admin also

                var emodel = new SendEmailViewModel()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Password = model.Password
                };

                if (result.Succeeded)
                {
                    email_list.Add(emodel);
                    this.SendEmail(email_list);

                    return RedirectToAction("AddAdmin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public void SendEmail(List<SendEmailViewModel> model)
        {
            foreach (var user in model)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Exam Portal", "exam.portal2022@gmail.com"));
                message.To.Add(new MailboxAddress(user.Name, user.UserName));
                message.Subject = "Login Credentials";
                message.Body = new TextPart("plain")
                {
                    Text = "Your Initial Login Credentials\n" +
                    "UserName: " + user.UserName +
                    "\nPassword: " + user.Password
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("exam.portal2022@gmail.com", "examportal@2022");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> ViewAdmin()
        {
            var model = await facultyRepository.GetAllFaculty("Admin");
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AdminDetails(int id)
        {
            var model = facultyRepository.GetFacultyDetails(id);

            return View(await model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> EditAdmin(int id)
        {
            var model = await facultyRepository.GetEditFaculty(id);
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> EditAdmin(FacultyDetailsViewModel model)
        {
            var result = await facultyRepository.PostEditFaculty(model);

            if (result.Succeeded)
            {
                return RedirectToAction("ViewAdmin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var result = await facultyRepository.DeleteFaculty(id);

            if (result.Succeeded)
            {
                return RedirectToAction("ViewAdmin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("ViewAdmin");
        }
    }
}
