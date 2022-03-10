using Exam_Portal.Enums;
using Exam_Portal.Models;
using Exam_Portal.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public StudentRepository(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateStudent(AddStudentViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                Password = model.Name + "@" + model.DOB.Substring(0, 4),
                Name = model.Name,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Branch = model.Branch.ToString(),
                Semester = model.Semester,
                Division = model.Division,
                DOB = model.DOB,
                InitialLogin = true
            };

            var result = await userManager.CreateAsync(user, user.Password);

            await userManager.AddToRoleAsync(user, model.Role.ToString());

            return result;
        }

        public async Task<ViewUserViewModel> GetAllStudent()
        {
            var model = new ViewUserViewModel();

            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, "Student"))
                {
                    model.User.Add(user);
                }
            }

            return model;
        }

        public async Task<IdentityResult> DeleteStudent(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            var result = await userManager.DeleteAsync(user);

            return result;
        }

        public async Task<StudentDetailsViewModel> GetStudentDetails(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            var model = new StudentDetailsViewModel
            {
                Email = user.Email,
                Name = user.Name,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Branch = (BranchEnum)Enum.Parse(typeof(BranchEnum), user.Branch), //casting string to enum
                Semester = user.Semester,
                Division = user.Division,
                DOB = user.DOB
            };

            return model;
        }

        public async Task<StudentDetailsViewModel> GetEditStudent(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            var model = new StudentDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Branch = (BranchEnum)Enum.Parse(typeof(BranchEnum), user.Branch), //casting string to enum
                Semester = user.Semester,
                Division = user.Division,
                DOB = user.DOB
            };

            return model;
        }

        public async Task<IdentityResult> PostEditStudent(StudentDetailsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Name = model.Name;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.Branch = model.Branch.ToString();
            user.Semester = model.Semester;
            user.Division = model.Division;
            user.DOB = model.DOB;

            var result = await userManager.UpdateAsync(user);

            return result;
        }
    }
}
