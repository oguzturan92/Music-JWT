using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Music_JWT_NetCore.Dtos;
using Music_JWT_NetCore.Models;
using Newtonsoft.Json;

namespace Music_JWT_NetCore.Controllers;

public class HomeController : Controller
{   
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {   
        string token = Request.Cookies["AuthToken"];
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync("https://localhost:7148/api/Musics");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<MusicDto>(jsonData);
            return View(values);
        }
        return View(new MusicDto(){PremiumName = "", Musics = new()});
    }

    public async Task<IActionResult> Query()
    {   
        string token = Request.Cookies["AuthToken"];
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync("https://localhost:7148/api/Musics");
        if (responseMessage.IsSuccessStatusCode)
        {
            return Json(responseMessage.StatusCode);
        }
        return Json(responseMessage.StatusCode);
    }
}
