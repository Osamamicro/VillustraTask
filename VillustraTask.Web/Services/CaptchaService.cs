using System.Drawing;
using System.Drawing.Imaging;

namespace VillustraTask.Web.Services
{
    public class CaptchaService
    {
        public byte[] GenerateCaptcha(out string captchaText, HttpContext context)
        {
            Random random = new Random();
            captchaText = random.Next(1000, 9999).ToString(); // Simple 4-digit Captcha

            // Store Captcha in session
            context.Session.SetString("CaptchaCode", captchaText);

            using var bitmap = new Bitmap(100, 40);
            using var graphics = Graphics.FromImage(bitmap);
            using var font = new Font("Arial", 18, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Black);
            using var backgroundBrush = new SolidBrush(Color.White);

            graphics.FillRectangle(backgroundBrush, 0, 0, bitmap.Width, bitmap.Height);
            graphics.DrawString(captchaText, font, brush, 10, 5);

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public bool ValidateCaptcha(string userInput, HttpContext context)
        {
            var storedCaptcha = context.Session.GetString("CaptchaCode");
            return storedCaptcha != null && storedCaptcha == userInput;
        }
    }
}
