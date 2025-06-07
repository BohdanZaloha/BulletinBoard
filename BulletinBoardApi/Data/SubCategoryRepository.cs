using BulletinBoardApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BulletinBoardApi.Data
{
    public class SubCategoryRepository(IConfiguration config) : ISubCategoryRepository
    {
        private string connectionString = config.GetConnectionString("DefaultConnection");

        async  Task<IEnumerable<SubCategory>> ISubCategoryRepository.GetAllSubCategoriesAsync()
        {
            var subCategories = new List<SubCategory>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("GetAllSubCategories", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                subCategories.Add(new SubCategory()
                {
                    SubCategoryId = (int)reader["SubCategoryId"],
                    CategoryId = (int)reader["CategoryId"],
                    Name = reader["Name"].ToString()!.Trim(),
                });
            }
            return subCategories;
        }

        async  Task<IEnumerable<SubCategory>> ISubCategoryRepository.GetSubCategoriesByCategoryIdAsync(int categoryId)
        {
            var subCategories = new List<SubCategory>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("GetSubCategoriesByCategoryId", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@CategoryId", categoryId);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                subCategories.Add(new SubCategory()
                {
                    SubCategoryId = (int)reader["SubCategoryId"],
                    CategoryId = (int)reader["CategoryId"],
                    Name = reader["Name"].ToString()!.Trim(),
                });
            }
            return subCategories;
        }
    }
}
