using System.ComponentModel.DataAnnotations;

namespace VillustraTask.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Captcha is required.")]
        public string CaptchaInput { get; set; } // User input for Captcha
    }
}
