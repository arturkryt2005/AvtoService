using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService.Data.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;

        public int Id { get; set; }

        public string Login { get; set; } = string.Empty;

        public string Role { get; set; } = "user";
    }
}
