using System.ComponentModel.DataAnnotations;

namespace VillustraTask.Api.Models
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        public string UserId { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        public int? DesignationId { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
