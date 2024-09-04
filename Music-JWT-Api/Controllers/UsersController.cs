using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Music_JWT_Api.Dtos;
using Music_JWT_Api.Models;

namespace Music_JWT_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _roleManager = roleManager;
        }

        [HttpPost("register")] // api/user/register
        public async Task<IActionResult> UserCreate(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(registerDto.PremiumId.ToString());
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                        return StatusCode(201);
                    }
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }
        
        [HttpPost("login")] // api/user/login
        public async Task<IActionResult> UserLogin(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return BadRequest(new { message = "email hatası" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(new { token = GenerateJWT(user, roles) });
            }
            return Unauthorized();
        }

        private object GenerateJWT(AppUser user, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Secret").Value ?? "");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(ClaimTypes.Name, user.UserName ?? ""),
                        new(ClaimTypes.Role, roles.FirstOrDefault())
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}