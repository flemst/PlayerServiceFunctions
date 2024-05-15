using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using System.Text.Json.Nodes;


namespace PlayerFunctions;

public class RestApi
{
  private static List<Player> _players = [];

  [Function("AddPlayer")]
  public IActionResult AddPlayer([HttpTrigger(AuthorizationLevel.Anonymous, "put", 
    Route = "v1/players")] HttpRequest req)
  {
    // Read player object from request
    string body = new StreamReader(req.Body).ReadToEnd();
    Player player = JsonConvert.DeserializeObject<Player>(body);

    // Validate player object
    if (player == null) 
      return new BadRequestObjectResult(string.Empty);

    // Add player to storage
    _players.Add(player);
    return new OkObjectResult(string.Empty);
  }

  [Function("GetPlayerById")]
  public IActionResult GetPlayerById([HttpTrigger(AuthorizationLevel.Anonymous, "get", 
    Route = "v1/players/{id}")] HttpRequest req, string id)
  {    
    // Find player in storage
    Player player = _players.Find(p => p.Id == id);

    // When player wasn't found
    if (player == null)
      return new NotFoundObjectResult(string.Empty);

    // Return the player json object
    string playerJsonString = JsonConvert.SerializeObject(player);
    return new OkObjectResult(playerJsonString);
  }

  [Function("GetPlayersByPosition")]
  public IActionResult GetPlayersByPosition([HttpTrigger(AuthorizationLevel.Anonymous, "get", 
    Route = "v1/players/position/{position}")] HttpRequest req, string position)
  {
    if (!VerifyPositionFormat(position))
      return new BadRequestObjectResult(string.Empty);

    IEnumerable<PositionPlayer> playersAtPosition = 
      _players.Where(p => p.Position == position)
              .Select(x => new PositionPlayer() { Id = x.Id, Name = x.Name });

    JsonObject jso = new JsonObject();
    string list = JsonConvert.SerializeObject(playersAtPosition);
    jso.Add("players", list);

    return new OkObjectResult(jso);
  }

  private bool VerifyPositionFormat(string position)
  {
    if (string.IsNullOrEmpty(position)) return false;
    return true;
  }
}

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

  [JsonProperty("position")]
  public string Position { get; set; }

  [JsonProperty("accessToken")]
  public string AccessToken { get; set; }
}

public class PositionPlayer
{
  [JsonProperty("playerId")]
  public string Id { get; set; }

  [JsonProperty("playerName")]
  public string Name { get; set; }
}
