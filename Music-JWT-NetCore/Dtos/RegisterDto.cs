using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music_JWT_NetCore.Dtos
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public int PremiumId { get; set; }
    }
}