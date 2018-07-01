using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oboete.Web.Models;

namespace Oboete.Web.Controllers
{
    public class VueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
