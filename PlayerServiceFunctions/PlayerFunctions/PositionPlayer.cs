using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PlayerFunctions;

public class PositionPlayer
{
  [JsonPropertyName("playerID")]
  public string Id { get; set; }

  [JsonPropertyName("playerName")]
  public string Name { get; set; }
}
