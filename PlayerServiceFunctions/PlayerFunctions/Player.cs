//using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PlayerFunctions;

public class Player
{
  [JsonPropertyName("playerID")]
  public string Id { get; set; }

  [JsonPropertyName("playerName")]
  public string Name { get; set; }

  [JsonPropertyName("groupName")]
  public string Group { get; set; }

  [JsonPropertyName("region")]
  public string Region { get; set; }

  [JsonPropertyName("positionAsString")]
  public string Position { get; set; }

  [JsonPropertyName("accessToken")]
  public string AccessToken { get; set; }
}
