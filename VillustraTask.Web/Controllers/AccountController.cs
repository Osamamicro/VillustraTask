using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VillustraTask.Web.Models;
using Microsoft.AspNetCore.Http;
using VillustraTask.Web.Services;
using System.Drawing;
using System.IO;
using CaptchaGen;
using VillustraTask.Api.Models;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace VillustraTask.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CaptchaService _captchaService;

        public AccountController(IHttpClientFactory httpClientFactory, CaptchaService captchaService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
            _captchaService = captchaService;
        }

        [HttpGet]
        public IActionResult GenerateCaptcha()
        {
            var random = new Random();
            var captchaText = random.Next(1000, 9999).ToString();
            _httpContextAccessor.HttpContext?.Session.SetString("CaptchaCode", captchaText);

            using var bitmap = new Bitmap(120, 40);
            using var graphics = Graphics.FromImage(bitmap);
            using var font = new Font("Arial", 18, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Black);
            using var backgroundBrush = new SolidBrush(Color.White);

            graphics.FillRectangle(backgroundBrush, 0, 0, bitmap.Width, bitmap.Height);
            graphics.DrawString(captchaText, font, brush, 10, 5);

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);

            return File(ms.ToArray(), "image/png");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var newUser = new RegisterUserRequest
            {
                UserId = model.Email,
                Password = model.Password,
                FullName = model.FullName,
                DesignationId = model.DesignationId,
                ProfilePicture = null
            };

            var json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7001/api/User/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Error = "Error registering user.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var random = new Random();
            var captchaCode = random.Next(1000, 9999).ToString();
            _httpContextAccessor.HttpContext?.Session.SetString("CaptchaCode", captchaCode);
            ViewBag.Captcha = captchaCode;
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var json = JsonConvert.SerializeObject(new { userId = model.Email, password = model.Password });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7001/api/User/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokenObj = JsonConvert.DeserializeObject<dynamic>(result);
                string token = tokenObj?.token?.ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    // Store the token in session for later API calls
                    _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", token);

                    // Create claims for the user (you can add more claims as needed)
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email)
                // You can add additional claims here if needed
            };

                    // Create the identity and principal
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in the user with cookie authentication
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    // Redirect to the Task management page
                    return RedirectToAction("Index", "Task");
                }
            }

            ViewBag.Error = "Invalid credentials!";
            return View(model);
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
