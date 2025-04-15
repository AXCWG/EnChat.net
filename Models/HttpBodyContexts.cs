using System.Text.Json.Serialization;

namespace EnChat.Models;

public static class HttpBodyContexts
{
    public class RegisterLoginContext
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class SendBaseContext
    {
        [JsonPropertyName("content")]
        public required string Content { get; set; }
     
    }

    public class SendContext : SendBaseContext
    {
        [JsonPropertyName("to")]
        public required string To { get; set; }
    }

  

    public class ProfileUpdateContext
    {
        [JsonPropertyName("profile")]
        public required byte[] Profile { get; set; } 
    }
}