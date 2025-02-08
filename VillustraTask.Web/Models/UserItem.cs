using System.ComponentModel.DataAnnotations;

namespace VillustraTask.Web.Models
{
    public class UserItem
    {
        [Required]
        [EmailAddress]
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        public int? DesignationId { get; set; }

        public bool IsLocked { get; set; }

        public bool IsLoggedIn { get; set; }

        public string? ProfilePicture { get; set; }

        public string CreatedBy { get; set; }
    }
}
