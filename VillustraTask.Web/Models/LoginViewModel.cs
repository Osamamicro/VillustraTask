namespace VillustraTask.Web.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CaptchaInput { get; set; } // User input for Captcha
    }
}
