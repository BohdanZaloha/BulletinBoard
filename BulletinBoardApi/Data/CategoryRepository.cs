using BulletinBoardApi.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BulletinBoardApi.Data
{
    public  class CategoryRepository(IConfiguration config): ICategoryRepository
    {
        private string connectionString = config.GetConnectionString("DefaultConnection");
       async Task<IEnumerable<Category>> ICategoryRepository.GetAllCategoriesAsync()
        {
           
            var categories = new List<Category>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("GetAllCategories", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            Category category = new Category();
            while (await reader.ReadAsync())
            {
                categories.Add(new Category()
                {
                    CategoryId = (int)reader["CategoryId"],
                    Name = reader["Name"].ToString()!.Trim(),
                });
            }
            return categories;

        }
    
    }
}
