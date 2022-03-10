using Exam_Portal.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Repository
{
    public interface IStudentRepository
    {
        Task<IdentityResult> CreateStudent(AddStudentViewModel model);
        Task<ViewUserViewModel> GetAllStudent();
        Task<StudentDetailsViewModel> GetStudentDetails(int id);
        Task<IdentityResult> DeleteStudent(int id);
        Task<StudentDetailsViewModel> GetEditStudent(int id);
        Task<IdentityResult> PostEditStudent(StudentDetailsViewModel model);
    }
}
