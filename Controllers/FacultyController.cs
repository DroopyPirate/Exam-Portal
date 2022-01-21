using Exam_Portal.Models;
using Exam_Portal.Repository;
using Exam_Portal.ViewModels;
using Exam_Portal.ViewModels.Admin;
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
    [Authorize]
    public class FacultyController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        private readonly IFacultyRepository facultyRepository;
        private readonly IStudentRepository studentRepository;

        public FacultyController(IFacultyRepository facultyRepository,
                               IStudentRepository studentRepository,
                               UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.facultyRepository = facultyRepository;
            this.studentRepository = studentRepository;
        }


        [Authorize(Roles = "Faculty")]
        public IActionResult Index()
        {
            return View();
        }




        [Authorize(Roles = "Faculty")]
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [Authorize(Roles = "Faculty")]
        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentViewModel model)
        {
            List<SendEmailViewModel> email_list = new List<SendEmailViewModel>();

            if (ModelState.IsValid)
            {
                var result = await studentRepository.CreateStudent(model);

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

                    return RedirectToAction("AddStudent");
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

        [Authorize(Roles = "Faculty")]
        [HttpGet]
        public IActionResult AddStudentExcel(List<ApplicationUser> model = null)
        {
            model = model == null ? new List<ApplicationUser>() : model;
            return View(model);
        }

        [Authorize(Roles = "Faculty")]
        [HttpPost]
        public async Task<IActionResult> AddStudentExcel(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<ApplicationUser> student = await this.GetStudentList(file.FileName);
            return View(student);
        }

        private async Task<List<ApplicationUser>> GetStudentList(string fName)
        {
            List<ApplicationUser> studentList = new List<ApplicationUser>();
            List<SendEmailViewModel> email_list = new List<SendEmailViewModel>();
            var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        var student = new ApplicationUser
                        {
                            Email = reader.GetValue(0).ToString(),
                            UserName = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Name = reader.GetValue(2).ToString(),
                            MiddleName = reader.GetValue(3).ToString(),
                            LastName = reader.GetValue(4).ToString(),
                            PhoneNumber = reader.GetValue(5).ToString(),
                            Address = reader.GetValue(6).ToString(),
                            Branch = reader.GetValue(7).ToString(),
                            Semester = Convert.ToInt32(reader.GetValue(8)),
                            Division = reader.GetValue(9).ToString(),
                            //DOB = reader.GetValue(10).ToString().Substring(0,10),
                            DOB = Convert.ToDateTime(reader.GetValue(10)).ToString("yyyy-MM-dd")
                        };

                        var emodel = new SendEmailViewModel()
                        {
                            Name = student.Name,
                            UserName = student.Email,
                            Password = student.Password
                        };
                        email_list.Add(emodel);

                        studentList.Add(student);

                        await userManager.CreateAsync(student, student.Password);

                        await userManager.AddToRoleAsync(student, "Student");

                    }
                }
                System.IO.File.Delete(fileName);
            }
            this.SendEmail(email_list);

            return studentList;
        }

        [Authorize(Roles = "Faculty")]
        [HttpGet]
        public async Task<IActionResult> ViewStudent()
        {
            var model = await studentRepository.GetAllStudent();
            return View(model);
        }

        [Authorize(Roles = "Faculty")]
        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await studentRepository.DeleteStudent(id);
            if (result.Succeeded)
            {
                return RedirectToAction("ViewStudent");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("ViewStudent");
        }

        [Authorize(Roles = "Faculty")]
        [HttpPost]
        public async Task<IActionResult> StudentDetails(int id)
        {
            var model = await studentRepository.GetStudentDetails(id);
            return View(model);
        }

        [Authorize(Roles = "Faculty")]
        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var model = await studentRepository.GetEditStudent(id);
            return View(model);
        }

        [Authorize(Roles = "Faculty")]
        [HttpPost]
        public async Task<IActionResult> EditStudent(StudentDetailsViewModel model)
        {
            var result = await studentRepository.PostEditStudent(model);

            if (result.Succeeded)
            {
                return RedirectToAction("ViewStudent");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
