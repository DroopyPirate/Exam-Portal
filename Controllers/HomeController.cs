using Exam_Portal.Enums;
using Exam_Portal.Models;
using Exam_Portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {

            if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                var user = await userManager.FindByEmailAsync(model.Email);
                var role = await userManager.GetRolesAsync(user);
                
                if(result.Succeeded)
                {
                    if(!user.InitialLogin)
                    {
                        foreach (string _role in role)
                        {
                            if (_role == "Admin")
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else if (_role == "Faculty")
                            {
                                return RedirectToAction("Index", "Faculty");
                            }
                            else if (_role == "Student")
                            {
                                return RedirectToAction("Index", "Student");
                            }
                            else
                            {
                                return RedirectToAction("Index", "SuperAdmin");
                            }
                        }
                    }
                    else
                    {
                        return View("ChangePassword");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                user.Password = model.NewPassword;
                user.InitialLogin = false;

                await userManager.UpdateAsync(user);
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                var role = await userManager.GetRolesAsync(user);

                foreach (string _role in role)
                {
                    if (_role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (_role == "Faculty")
                    {
                        return RedirectToAction("Index", "Faculty");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Student");
                    }
                }
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
