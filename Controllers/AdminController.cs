using Exam_Portal.Enums;
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
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Exam_Portal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly AppDbContext context;

        private readonly IFacultyRepository facultyRepository;
        private readonly IStudentRepository studentRepository;

        public AdminController(IFacultyRepository facultyRepository,
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

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }





        //Faculty



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddFaculty()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFaculty(AddFacultyViewModel model)
        {
            List<SendEmailViewModel> email_list = new List<SendEmailViewModel>();

            if(ModelState.IsValid)
            {
                var result = await facultyRepository.CreateFaculty(model);

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

                    return RedirectToAction("AddFaculty");
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
            foreach(var user in model)
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddFacultyExcel(List<ApplicationUser> model = null)
        {
            model = model == null ? new List<ApplicationUser>() : model;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFacultyExcel(IFormFile file, [FromServices]IHostingEnvironment hostingEnvironment)
        {
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";

            using(FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<ApplicationUser> faculty = await this.GetFacultyList(file.FileName);
            return View(faculty);
        }

        private async Task<List<ApplicationUser>> GetFacultyList(string fName)
        {
            List<ApplicationUser> facultyList = new List<ApplicationUser>();
            List<SendEmailViewModel> email_list = new List<SendEmailViewModel>();
            var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using(var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int counter = 0;
                    while(reader.Read())
                    {
                        if (counter == 0) { counter++; continue; } else counter++;
                        
                        var faculty = new ApplicationUser();
                        faculty.Email = reader.GetValue(0).ToString();
                        faculty.UserName = reader.GetValue(0).ToString();
                        //Password = reader.GetValue(1).ToString(),
                        faculty.Name = reader.GetValue(1).ToString();
                        faculty.MiddleName = reader.GetValue(2).ToString();
                        faculty.LastName = reader.GetValue(3).ToString();
                        faculty.PhoneNumber = reader.GetValue(4).ToString();
                        faculty.Address = reader.GetValue(5).ToString();
                        faculty.Branch = reader.GetValue(6).ToString();
                        //DOB = reader.GetValue(8).ToString().Substring(0,10),
                        faculty.DOB = Convert.ToDateTime(reader.GetValue(7)).ToString("yyyy-MM-dd");
                        faculty.Password = faculty.Name + "@" + faculty.DOB.Substring(0,4);
                        

                        var emodel = new SendEmailViewModel()
                        {
                            Name = faculty.Name,
                            UserName = faculty.Email,
                            Password = faculty.Password
                        };
                        email_list.Add(emodel);

                        facultyList.Add(faculty);

                        var result = await userManager.CreateAsync(faculty, faculty.Password);

                        var result2 = await userManager.AddToRoleAsync(faculty, "Faculty");

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

            return facultyList;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewFaculty()
        {
            var model = await facultyRepository.GetAllFaculty("Faculty");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteFaculty(int id)
        {
            var result = await facultyRepository.DeleteFaculty(id);

            if(result.Succeeded)
            {
                return RedirectToAction("ViewFaculty");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("ViewFaculty");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> FacultyDetails(int id)
        {
            var model = facultyRepository.GetFacultyDetails(id);

            return View( await model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditFaculty(int id)
        {
            var model = await facultyRepository.GetEditFaculty(id);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditFaculty(FacultyDetailsViewModel model)
        {
            var result = await facultyRepository.PostEditFaculty(model);

            if(result.Succeeded)
            {
                return RedirectToAction("ViewFaculty");
            }
            
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }




        //Student



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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
                    Password = model.Name + "@" + model.DOB.Substring(0,4),
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddStudentExcel(List<ApplicationUser> model = null)
        {
            model = model == null ? new List<ApplicationUser>() : model;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
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
                        //Password = reader.GetValue(1).ToString();
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

                        if(!(result.Succeeded && result2.Succeeded))
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewStudent()
        {
            var model = await studentRepository.GetAllStudent();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> StudentDetails(int id)
        {
            var model = await studentRepository.GetStudentDetails(id);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var model = await studentRepository.GetEditStudent(id);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> ViewMyTest()
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var model = new ViewTestViewModel();  
            var tests = (from t in context.Tests 
                         where t.Faculty_id == user_id select t).ToList(); //Get all Tests
            List<int> inActiveList = new();

            var currentDate = DateTime.Now;
            foreach (var t in tests)
            {
                if (t.EndDate != "-") 
                {
                    var endDate = Convert.ToDateTime(t.EndDate);
                    if (t.isActive == false)
                    {
                        inActiveList.Add(tests.IndexOf(t));
                    }
                    else if (currentDate > endDate)
                    {
                        t.isActive = false;
                        context.SaveChanges();
                        inActiveList.Add(tests.IndexOf(t));
                    }
                }               
            }

            foreach(int i in inActiveList)
            {
                tests.RemoveAt(i);
            }

            foreach (var test in tests)
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
        public async Task<IActionResult> ViewOtherTest()
        {
            int user_id = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var model = new ViewTestViewModel();
            var tests = (from t in context.Tests
                         where t.Faculty_id != user_id
                         select t).ToList(); //Get all Tests
            List<int> inActiveList = new();

            var currentDate = DateTime.Now;
            foreach (var t in tests)
            {
                if (t.EndDate != "-")
                {
                    var endDate = Convert.ToDateTime(t.EndDate);
                    if (t.isActive == false)
                    {
                        inActiveList.Add(tests.IndexOf(t));
                    }
                    else if (currentDate > endDate)
                    {
                        t.isActive = false;
                        context.SaveChanges();
                        inActiveList.Add(tests.IndexOf(t));
                    }
                }
            }

            foreach (int i in inActiveList)
            {
                tests.RemoveAt(i);
            }

            foreach (var test in tests)
            {
                var creator = await userManager.FindByIdAsync(test.Faculty_id.ToString());
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
    }
}