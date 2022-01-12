using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Portal.Controllers
{
    //[Authorize]
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
