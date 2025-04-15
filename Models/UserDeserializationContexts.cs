using System.Text.Json.Serialization;

namespace EnChat.Models;

public static class UserDeserializationContexts
{
    public class Contacts
    {
        [JsonPropertyName("uuid")]
        public required string Uuid { get; set;  }
        [JsonPropertyName("dateMet")]
        public required DateTimeOffset DateMet { get; set;  }
        
    }

    // public class Pending : HttpBodyContexts.SendContext
    // {
    //     [JsonPropertyName("uuid")]
    //     public required string Uuid { get; set;  }
    //     [JsonPropertyName("date")]
    //     public required DateTimeOffset Date { get; set; }
    //     [JsonPropertyName("from")]
    //     public required string From { get; set; }
    //     
    // }
}