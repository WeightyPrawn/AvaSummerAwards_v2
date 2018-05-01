using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Add([FromBody] NewNominationViewModel nomination)
        {
            try
            {
                await _nominationRepository.Add(nomination, User, HttpContext.Session);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpDelete("{nominationId:int}")]
        public IActionResult Delete(int nominationId)
        {
            _nominationRepository.Delete(nominationId, User.Identity.Name);
            return Ok();
        }

    }
}
