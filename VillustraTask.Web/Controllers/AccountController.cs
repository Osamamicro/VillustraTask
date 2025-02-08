using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VillustraTask.Web.Models;
using VillustraTask.Web.Services;

namespace VillustraTask.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CaptchaService _captchaService;

        public AccountController(IHttpClientFactory httpClientFactory, CaptchaService captchaService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _captchaService = captchaService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GenerateCaptcha()
        {
            string captchaText;
            var captchaBytes = _captchaService.GenerateCaptcha(out captchaText, HttpContext);
            return File(captchaBytes, "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Validate Captcha
            if (!_captchaService.ValidateCaptcha(model.CaptchaInput, HttpContext))
            {
                ViewBag.Error = "Captcha verification failed!";
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7001/api/User/login", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokenObj = JsonConvert.DeserializeObject<dynamic>(result);
                string token = tokenObj?.token?.ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    HttpContext.Session.SetString("JWTToken", token);
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Invalid credentials!";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
