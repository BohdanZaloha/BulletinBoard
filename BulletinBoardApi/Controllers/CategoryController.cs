using BulletinBoardApi.Data;
using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardApi.Controllers
{

    /// <summary>
    /// Provides endpoints to retrieve <see cref="Category"/> entities.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository _repository) : ControllerBase
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repository.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
