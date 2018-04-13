using System.Diagnostics;
using Awards.Repositories;
using Awards.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awards.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private CategoryRepository _categoryRepository { get; set; }

        public AdminController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            return Json(_categoryRepository.GetCategories());
        }

        public IActionResult AddCategory(EditCategoryViewModel data)
        {
            _categoryRepository.Add(data);
            return Ok();
        }

        public IActionResult EditCategory(EditCategoryViewModel data)
        {
            _categoryRepository.Update(data);
            return Ok();
        }

        public IActionResult DeleteCategory(int categoryId)
        {
            _categoryRepository.Remove(categoryId);
            return Ok();
        }
    }
}
