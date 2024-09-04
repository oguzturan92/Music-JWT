using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Music_JWT_NetCore.Dtos;
using Newtonsoft.Json;

namespace Music_JWT_NetCore.Controllers
{
    public class RegistersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegistersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(registerDto);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("https://localhost:7148/api/Users/register", stringContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return Json(responseMessage.StatusCode);
                }
                return Json(responseMessage.StatusCode);
            }
            return Json(registerDto);
        }
    }
}