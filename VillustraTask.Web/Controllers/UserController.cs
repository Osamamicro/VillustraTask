using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VillustraTask.Web.Models;

namespace VillustraTask.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _httpClient.GetAsync("https://localhost:7001/api/User/list");
            if (!response.IsSuccessStatusCode)
                return BadRequest("Failed to load users.");

            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] UserItem user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            if (!string.IsNullOrEmpty(user.UserId))
                response = await _httpClient.PutAsync("https://localhost:7001/api/User/update", content);
            else
                response = await _httpClient.PostAsync("https://localhost:7001/api/User/register", content);

            return response.IsSuccessStatusCode ? Ok() : BadRequest("Error saving user.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7001/api/User/delete/{userId}");
            return response.IsSuccessStatusCode ? Ok() : BadRequest("Error deleting user.");
        }
    }
}
