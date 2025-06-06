using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace BulletinBoardApi.Data
{
    public class AnnouncementRepository(IConfiguration config): IAnnouncementRepository
    {
        private string connectionString = config.GetConnectionString("DefaultConnection");
       // private readonly BulletinBoardDbContext _context;

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
                   Id = reader.GetInt32(0),
                   Title = reader.GetString(1),
                   Description = reader.GetString(2),
                   CreatedDate = reader.GetDateTime(3),
                   Status = reader.GetBoolean(4),
                   CategoryId = reader.GetInt32(5),
                   SubCategoryId = reader.GetInt32(6)
               };
            }
            return announcement;
        }

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
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedDate = reader.GetDateTime(3),
                    Status = reader.GetBoolean(4),
                    CategoryId = reader.GetInt32(5),
                    SubCategoryId = reader.GetInt32(6)
                });
            }
            return announcements;

        }

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
