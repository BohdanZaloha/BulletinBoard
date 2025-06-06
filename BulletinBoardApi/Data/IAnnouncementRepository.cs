using BulletinBoardApi.Models;

namespace BulletinBoardApi.Data
{
    public interface IAnnouncementRepository
    {
        public Task<IEnumerable<Announcement>> GetAnnouncementsAsync();
        public Task<Announcement?> GetAnnouncementByIdAsync(int id);
        public Task CreateAnnouncementAsync(Announcement announcement);
        public Task UpdateAnnouncementAsync(Announcement announcement);
        public Task DeleteAnnouncementAsync(int id);  

    }
}
