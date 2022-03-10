using Exam_Portal.ViewModels;
using Exam_Portal.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Repository
{
    public interface IFacultyRepository
    {
        Task<IdentityResult> CreateFaculty(AddFacultyViewModel model);
        Task<ViewUserViewModel> GetAllFaculty(string role);
        Task<FacultyDetailsViewModel> GetFacultyDetails(int id);
        Task<IdentityResult> DeleteFaculty(int id);
        Task<FacultyDetailsViewModel> GetEditFaculty(int id);
        Task<IdentityResult> PostEditFaculty(FacultyDetailsViewModel model);
    }
}
