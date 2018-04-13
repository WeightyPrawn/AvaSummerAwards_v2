using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AvaSummerAwards_v2.Controllers
{
    public class NominateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}