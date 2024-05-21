using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlayerFunctions;

public class RestApi
{
  private IStorageConnector _storage = new InMemoryStorage();
  private readonly ILogger<RestApi> _logger;

  public RestApi(ILogger<RestApi> logger)
  {
    _logger = logger;
  }


  [Function("AddPlayer")]
  public async Task<IActionResult> AddPlayer(
  [HttpTrigger(AuthorizationLevel.Anonymous, "put",
    Route = "v1/players")] HttpRequest req)
  {
    // Read player object from request
    string body = await new StreamReader(req.Body).ReadToEndAsync();
    _logger.LogInformation($"Body: {body}");
    //Player player = JsonConvert.DeserializeObject<Player>(body);
    Player player = JsonSerializer.Deserialize<Player>(body);

    // Validate player object
    if (player == null)
      return new BadRequestObjectResult(string.Empty);

    // Add player to storage
    _logger.LogInformation($"Adding player to storage. \n  Name: {player.Name}\n  Id: {player.Id}");
    _storage.Add(player);


    var res = new OkObjectResult(player);

    return new OkObjectResult(player);
  }

  [Function("GetPlayerById")]
  public IActionResult GetPlayerById(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get",
    Route = "v1/players/{id}")] HttpRequest req, string id)
  {
    // Find player in storage
    Player player = _storage.GetById(id);

    // When player wasn't found
    if (player == null)
      return new NotFoundObjectResult(string.Empty);

    // Return the player json object
    string playerJsonString = JsonSerializer.Serialize(player);
    return new OkObjectResult(playerJsonString);
  }

  [Function("GetPlayersByPosition")]
  public IActionResult GetPlayersByPosition(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get",
    Route = "v1/players/position/{position}")] HttpRequest req, string position)
  {
    if (!VerifyPositionFormat(position))
      return new BadRequestObjectResult(string.Empty);



    JsonArray arr = new JsonArray();


    IEnumerable<PositionPlayer> playersAtPosition = _storage.GetByPosition(position);


    foreach (PositionPlayer player in playersAtPosition)
    {
      JsonObject jo = new JsonObject();
      jo.Add("playerId", player.Id);
      jo.Add("playerName", player.Name);
      arr.Add(jo);
    }

    JsonObject jsonObj = new JsonObject();

    jsonObj.Add("players", arr);

    //JsonObject jsonObject = new JsonObject();

    //JsonCon

    //string players = JsonConvert.SerializeObject(playersAtPosition);
    //jsonObject.Add("players", players);

    return new OkObjectResult(jsonObj);
  }

  private bool VerifyPositionFormat(string position)
  {
    if (string.IsNullOrEmpty(position)) return false;
    return true;
  }
}
