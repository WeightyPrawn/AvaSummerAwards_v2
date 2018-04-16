using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awards.Repositories;
using Awards.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvaSummerAwards_v2.Controllers
{
    public class NominateController : Controller
    {
        private CategoryRepository _categoryRepository { get; set; }
        private NominationRepository _nominationRepository { get; set; }

        public NominateController(CategoryRepository categoryRepository, NominationRepository nominationRepository)
        {
            _categoryRepository = categoryRepository;
            _nominationRepository = nominationRepository;
        }

        public IActionResult Index()
        {
            var nominations = _categoryRepository.GetNominationsForUser(User.Identity.Name);
            return View(nominations);
        }

        [HttpPost]
        public IActionResult AddNomination(NewNominationViewModel nomination)
        {
            _nominationRepository.Add(nomination, User.Identity.Name);
            return RedirectToAction("Index");
        }
    }
}