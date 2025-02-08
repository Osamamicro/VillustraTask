using CaptchaGen;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;

namespace VillustraTask.Web.Services
{
    public class CaptchaService
    {
        public byte[] GenerateCaptcha(out string captchaText, HttpContext context)
        {
            // Generate a random 4-digit number
            Random random = new Random();
            captchaText = random.Next(1000, 9999).ToString();

            // Store Captcha in session
            context.Session.SetString("CaptchaCode", captchaText);

            using var memoryStream = new MemoryStream();
            using var bitmap = new Bitmap(100, 40);
            using var graphics = Graphics.FromImage(bitmap);
            using var font = new Font("Arial", 18, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Black);
            using var backgroundBrush = new SolidBrush(Color.White);

            graphics.FillRectangle(backgroundBrush, 0, 0, bitmap.Width, bitmap.Height);
            graphics.DrawString(captchaText, font, brush, 10, 5);

            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return memoryStream.ToArray();
        }

        public bool ValidateCaptcha(string userInput, HttpContext context)
        {
            var storedCaptcha = context.Session.GetString("CaptchaCode");
            return storedCaptcha != null && storedCaptcha == userInput;
        }
    }
}
