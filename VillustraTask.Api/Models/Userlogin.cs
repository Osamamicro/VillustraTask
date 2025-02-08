using System;

namespace VillustraTask.Api.Models
{
    public class Userlogin
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int? DesignationId { get; set; }

        public bool IsLocked { get; set; } = false;
        public bool IsLoggedIn { get; set; } = false;

        public string? ProfilePicture { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
