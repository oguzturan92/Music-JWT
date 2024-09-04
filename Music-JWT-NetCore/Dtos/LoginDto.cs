using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music_JWT_NetCore.Dtos
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}