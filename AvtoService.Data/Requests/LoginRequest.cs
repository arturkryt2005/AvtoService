using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AvtoService.Data.Requests
{
    public class LoginRequest
    {
        [JsonPropertyName("Login")]
        public string? Login { get; set; }

        [JsonPropertyName("Password")]
        public string? Password { get; set; }

    }
}
