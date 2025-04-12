namespace EnChat.Models;

public static class HttpBodyContexts
{
    public class RegisterLoginContext
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}