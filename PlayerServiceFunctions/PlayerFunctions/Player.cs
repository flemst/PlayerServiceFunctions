using Newtonsoft.Json;

namespace PlayerFunctions;

public class Player
{
  [JsonProperty("playerId")]
  public string Id { get; set; }

  [JsonProperty("playerName")]
  public string Name { get; set; }

  [JsonProperty("groupName")]
  public string Group { get; set; }

  [JsonProperty("region")]
  public string Region { get; set; }

  [JsonProperty("positionAsString")]
  public string Position { get; set; }

  [JsonProperty("accessToken")]
  public string AccessToken { get; set; }
}
