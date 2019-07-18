using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application1.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult MyProfile()
        {
            return View();
        }
    }
}