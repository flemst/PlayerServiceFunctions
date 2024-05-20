using Newtonsoft.Json;

namespace PlayerFunctions;

public class PositionPlayer
{
  [JsonProperty("playerId")]
  public string Id { get; set; }

  [JsonProperty("playerName")]
  public string Name { get; set; }
}
