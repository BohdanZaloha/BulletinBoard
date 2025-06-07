using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoardApi.Models
{
    [Table("Announcements")]
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string  Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }

        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }

    }
}
