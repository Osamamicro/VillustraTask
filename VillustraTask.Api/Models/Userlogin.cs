namespace VillustraTask.Api.Models
{
    public class Userlogin
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int? DesignationId { get; set; }      // Role/Designation ID 

        public bool IsLocked { get; set; } = false;  // Default: Not locked
        public bool IsLoggedIn { get; set; } = false;// Default: Not logged in

        public string? ProfilePicture { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;   // Default to current time

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
