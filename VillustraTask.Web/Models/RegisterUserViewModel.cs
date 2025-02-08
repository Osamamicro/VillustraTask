using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VillustraTask.Web.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int? DesignationId { get; set; }

        public List<SelectListItem> DesignationList { get; set; } = new List<SelectListItem>();
    }
}
