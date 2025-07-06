using System.ComponentModel.DataAnnotations;

namespace MiniStop.Models
{
    public class User
    {
        [Key]
        public int Usrid { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
    }

}
