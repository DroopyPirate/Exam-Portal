using Exam_Portal.Models;
using Exam_Portal.ViewModels;
using Exam_Portal.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AdminController(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

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
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Branch = model.Branch.ToString(),
                    DOB = model.DOB
                };

                var result = await userManager.CreateAsync(user, model.Password);

                var role_result = await userManager.AddToRoleAsync(user, model.Role.ToString());

                if(result.Succeeded && role_result.Succeeded)
                {
                    return RedirectToAction("AddFaculty");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewFaculty()
        {
            //var role = await roleManager.FindByIdAsync("1");

            var model = new ViewFacultyViewModel();

            foreach(var user in userManager.Users.ToList())
            {
                if(await userManager.IsInRoleAsync(user, "Faculty"))
                {
                    model.User.Add(user);
                }
            }

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteFaculty(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound"); //This view dosent existy right now
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

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
        }

        [HttpPost]
        public async Task<IActionResult> FacultyDetails(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            var model = new FacultyDetailsViewModel();

            model.Email = user.Email;
            model.Name = user.Name;
            model.MiddleName = user.MiddleName;
            model.LastName = user.LastName;
            model.PhoneNumber = user.PhoneNumber;
            model.Address = user.Address;
            model.Branch = user.Branch;
            model.DOB = user.DOB;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditFaculty(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            var model = new FacultyDetailsViewModel();

            model.Id = user.Id;
            model.Email = user.Email;
            model.Name = user.Name;
            model.MiddleName = user.MiddleName;
            model.LastName = user.LastName;
            model.PhoneNumber = user.PhoneNumber;
            model.Address = user.Address;
            model.Branch = user.Branch;
            model.DOB = user.DOB;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditFaculty(FacultyDetailsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Name = model.Name;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            model.PhoneNumber = model.PhoneNumber;
            model.Address = model.Address;
            model.Branch = model.Branch;
            user.DOB = model.DOB;

            var result = await userManager.UpdateAsync(user);

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
    }
}
