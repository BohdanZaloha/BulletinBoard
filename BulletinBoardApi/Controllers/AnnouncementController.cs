using BulletinBoardApi.Data;
using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BulletinBoardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController(IAnnouncementRepository _repository) : ControllerBase
    {
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] Announcement announcement)
        {
            if (announcement == null)
            {
                return BadRequest("Announcement cannot be null.");
            }
            await _repository.CreateAnnouncementAsync(announcement);
            return CreatedAtAction(nameof(GetById), new { id = announcement.Id }, announcement);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAll()
        {
            var announcements = await _repository.GetAnnouncementsAsync();
            return Ok(announcements);
        }


        [HttpGet("GetAnnouncementById/{id:int}")]
        public async Task<ActionResult<Announcement>> GetById(int id)
        {
            var announcement = await _repository.GetAnnouncementByIdAsync(id);
            if (announcement == null)
            {
                return NotFound($"Announcement with ID {id} not found.");
            }
            return Ok(announcement);

        }
        [HttpPut("UpdateAnnouncement/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Announcement announcement)
        {
            await _repository.UpdateAnnouncementAsync(announcement);
            return NoContent();
        }
        [HttpDelete("DeleteAnnouncement/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var announcement = await _repository.GetAnnouncementByIdAsync(id);
            if (announcement == null)
            {
                return NotFound($"Announcement with ID {id} not found.");
            }
            await _repository.DeleteAnnouncementAsync(id);
            return NoContent();

        }
    }
}
