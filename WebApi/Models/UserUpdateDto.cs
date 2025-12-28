using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
    }
}