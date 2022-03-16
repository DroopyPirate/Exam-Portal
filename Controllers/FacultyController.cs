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
        private readonly AppDbContext context;

        private readonly IFacultyRepository facultyRepository;
        private readonly IStudentRepository studentRepository;

        public FacultyController(IFacultyRepository facultyRepository,
                               IStudentRepository studentRepository,
                               UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               RoleManager<ApplicationRole> roleManager,
                               AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.facultyRepository = facultyRepository;
            this.studentRepository = studentRepository;
            this.context = context;
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
                    Password = model.Name + "@" + model.DOB.Substring(0, 4),
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
                    int counter = 0;
                    while (reader.Read())
                    {
                        if (counter == 0) { counter++; continue; } else { counter++; }

                        var student = new ApplicationUser();
                        student.Email = reader.GetValue(0).ToString();
                        student.UserName = reader.GetValue(0).ToString();
                        //student.Password = reader.GetValue(1).ToString();
                        student.Name = reader.GetValue(1).ToString();
                        student.MiddleName = reader.GetValue(2).ToString();
                        student.LastName = reader.GetValue(3).ToString();
                        student.PhoneNumber = reader.GetValue(4).ToString();
                        student.Address = reader.GetValue(5).ToString();
                        student.Branch = reader.GetValue(6).ToString();
                        student.Semester = Convert.ToInt32(reader.GetValue(7));
                        student.Division = reader.GetValue(8).ToString();
                        //DOB = reader.GetValue(10).ToString().Substring(0,10),
                        student.DOB = Convert.ToDateTime(reader.GetValue(9)).ToString("yyyy-MM-dd");
                        student.Password = student.Name + "@" + student.DOB.Substring(0,4);
                        

                        var emodel = new SendEmailViewModel()
                        {
                            Name = student.Name,
                            UserName = student.Email,
                            Password = student.Password
                        };
                        email_list.Add(emodel);

                        studentList.Add(student);

                        var result = await userManager.CreateAsync(student, student.Password);

                        var result2 = await userManager.AddToRoleAsync(student, "Student");

                        if (!(result.Succeeded && result2.Succeeded))
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }

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




        [HttpGet]
        public async Task<IActionResult> ViewTest()
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var model = new ViewTestViewModel();
            var tests = (from t in context.Tests 
                         where t.Faculty_id == user_id
                         select t).ToList(); //Get all Tests
            var creator = await userManager.FindByIdAsync(user_id.ToString());

            foreach (var test in tests)
            {
                int count = (from tq in context.TestQuestions
                             where tq.Test_id == test.Id
                             select tq).ToList().Count();

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
        public IActionResult TestResult()
        {
            var userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var tests = (from t in context.Tests
                         where t.Faculty_id == userId && t.StartDate != "-" && t.EndDate != "-"
                         select t).ToList(); //Get all tests created by this user

            // Remove tests that are not started yet.
            var currentDate = DateTime.Now;
            List<int> removeList = new();
            foreach (var t in tests)
            {
                var startDate = Convert.ToDateTime(t.StartDate);
                if (startDate > currentDate)
                {
                    removeList.Add(tests.IndexOf(t));
                }
            }
            foreach (var i in removeList)
            {
                tests.RemoveAt(i);
            }

            string Creator = "";
            int noOfQuestions = 0;
            string type = "";
            var model = new ViewTestViewModel();

            foreach (var t in tests)
            {
                var user = context.ApplicationUser.Find(t.Faculty_id);
                Creator = user.Name + " " + user.LastName;
                noOfQuestions = (from tq in context.TestQuestions
                                 where tq.Test_id == t.Id
                                 select tq.Id).Count();
                type = (from tt in context.TestTypes
                        where tt.Id == t.Type_id
                        select tt.Type_name).ToList()[0];
                var extendedTest = new TestExtended
                {
                    Id = t.Id,
                    Title = t.Title,
                    CreatorName = Creator,
                    NoOfQuestions = noOfQuestions,
                    Type_name = type,
                };
                model.TestExtendeds.Add(extendedTest);
            }

            return View(model);
        }
    }
}
