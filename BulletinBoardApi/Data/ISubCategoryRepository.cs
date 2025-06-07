using BulletinBoardApi.Models;

namespace BulletinBoardApi.Data
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync();
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
    }
}
