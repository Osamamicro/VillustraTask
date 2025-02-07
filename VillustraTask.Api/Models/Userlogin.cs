namespace VillustraTask.Api.Models
{
    public class Userlogin
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int? DesignationId { get; set; }
        public bool Islocked { get; set; }
        public bool IsLoggedIn { get; set; }
        public string ProfilePicture { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
