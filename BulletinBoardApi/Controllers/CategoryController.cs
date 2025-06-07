using BulletinBoardApi.Data;
using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BulletinBoardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository _repository) : ControllerBase
    {

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repository.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
