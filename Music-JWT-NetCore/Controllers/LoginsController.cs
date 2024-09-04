using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Music_JWT_NetCore.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Music_JWT_NetCore.Controllers
{
    public class LoginsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(loginDto);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("https://localhost:7148/api/Users/login", stringContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var tokenJson = await responseMessage.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(tokenJson);
                    string token = (string)jsonObject["token"];
                    CookieOptions options = new()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddMinutes(1)
                    };
                    Response.Cookies.Append("AuthToken", token, options);
                    
                    return Json(responseMessage.StatusCode);
                }
                return Json(responseMessage.StatusCode);
            }
            return Json(loginDto);
        }
    }
}