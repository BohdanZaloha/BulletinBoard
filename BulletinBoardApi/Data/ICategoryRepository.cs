using BulletinBoardApi.Models;

namespace BulletinBoardApi.Data
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
