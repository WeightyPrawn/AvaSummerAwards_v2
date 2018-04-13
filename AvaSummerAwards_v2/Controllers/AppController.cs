using System.Diagnostics;
using Awards.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awards.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles="admin")]
        public IActionResult Admin()
        {
            return View();
        }
    }
}
