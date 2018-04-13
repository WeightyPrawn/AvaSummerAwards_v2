using System.Diagnostics;
using Awards.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awards.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private CategoryRepository _categoryRepository { get; set; }

        public CategoriesController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            return Json(_categoryRepository.GetCategoriesForUser(User.Identity.Name));
        }
    }
}
