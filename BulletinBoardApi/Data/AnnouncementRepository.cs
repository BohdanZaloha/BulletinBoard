using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace BulletinBoardApi.Data
{
    /// <summary>
    /// Repository implementation for managing <see cref="Announcement"/> entities
    /// via stored procedures in the SQL database.
    /// </summary>
    public class AnnouncementRepository(IConfiguration config): IAnnouncementRepository
    {
        private readonly string connectionString = config.GetConnectionString("DefaultConnection");

        /// <summary>
        /// Inserts a new announcement into the database.
        /// </summary>
        public async Task CreateAnnouncementAsync(Announcement announcement)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("AddAnnouncement", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Title", announcement.Title);
            command.Parameters.AddWithValue("@Description", announcement.Description);
            command.Parameters.AddWithValue("@Status", announcement.Status);
            command.Parameters.AddWithValue("@CategoryId", announcement.CategoryId);
            command.Parameters.AddWithValue("@SubCategoryId", announcement.SubCategoryId);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Removes an existing announcement by its identifier.
        /// </summary>
        public async Task DeleteAnnouncementAsync(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("DeleteAnnouncementById", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Retrieves a single announcement by its identifier.
        /// </summary>
        public async Task<Announcement> GetAnnouncementByIdAsync(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("GetAnnouncementById", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            Announcement announcement = new Announcement();
            while (await reader.ReadAsync()) 
            {
                announcement = new Announcement()
               {
                    Id = (int)reader["Id"],
                    Title = reader["Title"].ToString()!,
                    Description = reader["Description"] as string,
                    CreatedDate = (DateTime)reader["CreatedDate"],
                    Status = (bool)reader["Status"],
                    CategoryId = (int)reader["CategoryId"],
                    SubCategoryId = (int)reader["SubCategoryId"],
                    CategoryName = reader["CategoryName"].ToString().Trim(),
                    SubCategoryName = reader["SubCategoryName"].ToString().Trim()
                };
            }
            return announcement;
        }

        /// <summary>
        /// Retrieves all announcements from the database.
        /// </summary>
        public async Task<IEnumerable<Announcement>> GetAnnouncementsAsync()
        {
            var announcements = new List<Announcement>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("GetAllAnnouncements", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            Announcement announcement = new Announcement();
            while (await reader.ReadAsync())
            {
                announcements.Add( new Announcement()
                {
                    Id = (int)reader["Id"],
                    Title = reader["Title"].ToString()!,
                    Description = reader["Description"] as string,
                    CreatedDate = (DateTime)reader["CreatedDate"],
                    Status = (bool)reader["Status"],
                    CategoryId = (int)reader["CategoryId"],
                    SubCategoryId = (int)reader["SubCategoryId"],
                    CategoryName = reader["CategoryName"].ToString().Trim(),
                    SubCategoryName = reader["SubCategoryName"].ToString().Trim()
                });
            }
            return announcements;

        }

        /// <summary>
        /// Updates an existing announcement in the database.
        /// </summary>
        public async Task UpdateAnnouncementAsync(Announcement announcement)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("UpdateAnnouncement", connection) //dbo.
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", announcement.Id);
            command.Parameters.AddWithValue("@Title", announcement.Title);
            command.Parameters.AddWithValue("@Description", announcement.Description);
            command.Parameters.AddWithValue("@Status", announcement.Status);
            command.Parameters.AddWithValue("@CategoryId", announcement.CategoryId);
            command.Parameters.AddWithValue("@SubCategoryId", announcement.SubCategoryId);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
