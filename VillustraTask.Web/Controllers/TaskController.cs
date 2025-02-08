using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using VillustraTask.Web.Models;

[Authorize]
public class TaskController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TaskController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
        if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("https://localhost:7001/api/Task");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<List<TaskItem>>(json);
            return View(tasks);
        }

        ViewBag.Error = "Failed to load tasks.";
        return View(new List<TaskItem>());
    }

    [HttpPost]
    public async Task<IActionResult> SaveTask([FromBody] TaskItem task)
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
        if (string.IsNullOrEmpty(token)) return Unauthorized();

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var json = JsonConvert.SerializeObject(task);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response;
        if (task.TaskId > 0)
            response = await _httpClient.PutAsync("https://localhost:7001/api/Task/update", content);
        else
            response = await _httpClient.PostAsync("https://localhost:7001/api/Task/create", content);

        return response.IsSuccessStatusCode ? Ok() : BadRequest("Error saving task.");
    }
}
