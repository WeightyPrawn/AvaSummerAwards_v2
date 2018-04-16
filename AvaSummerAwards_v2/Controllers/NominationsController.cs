using System.Diagnostics;
using Awards.Repositories;
using Awards.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awards.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NominationsController : Controller
    {
        private CategoryRepository _categoryRepository { get; set; }
        private NominationRepository _nominationRepository { get; set; }

        public NominationsController(CategoryRepository categoryRepository, NominationRepository nominationRepository)
        {
            _categoryRepository = categoryRepository;
            _nominationRepository = nominationRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_categoryRepository.GetNominationsForUser(User.Identity.Name));
        }

        [HttpPost]
        public IActionResult Add([FromBody] NewNominationViewModel nomination)
        {
            _nominationRepository.Add(nomination, User.Identity.Name);
            return Ok();
        }

        [HttpDelete("{nominationId:int}")]
        public IActionResult Delete(int nominationId)
        {
            _nominationRepository.Delete(nominationId, User.Identity.Name);
            return Ok();
        }

    }
}
