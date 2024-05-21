using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlayerFunctions;

public class RestApi
{  
  private ILogger<RestApi> _logger;
  private IStorageConnector _storage;

  public RestApi(ILogger<RestApi> logger, IStorageConnector storage)
  {
    _logger = logger;
    _storage = storage;
  }


  [Function("AddPlayer")]
  public async Task<IActionResult> AddPlayer(
    [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/players")] HttpRequest req)
  {
    // Read player object from request
    string body = await new StreamReader(req.Body).ReadToEndAsync();
    Player player = JsonSerializer.Deserialize<Player>(body);

    // Validate player object
    if (player == null)
      return new BadRequestObjectResult(string.Empty);

    // Add player to storage
    _logger.LogInformation($"Adding player to storage. \n  Name: {player.Name}\n  Id: {player.Id}");
    _storage.AddOrUpdate(player);

    return new OkObjectResult(string.Empty);
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
    //string playerJsonString = JsonSerializer.Serialize(player);


    //var res = new OkObjectResult(playerJsonString);
    //var jsonResult = new ContentResult
    //{
    //  Content = JsonSerializer.Serialize(player),
    //  ContentType = MediaTypeNames.Application.Json,
    //  StatusCode = StatusCodes.Status200OK
    //};

    //return jsonResult;
    //return new OkObjectResult(playerJsonString);

    return new ContentResult
    {
      Content = JsonSerializer.Serialize(player),
      ContentType = MediaTypeNames.Application.Json,
      StatusCode = StatusCodes.Status200OK
    };
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
      JsonObject jo = new JsonObject
      {
        { "playerId", player.Id },
        { "playerName", player.Name }
      };
      arr.Add(jo);
    }

    JsonObject jsonObj = new JsonObject
    {
      { "players", arr }
    };

    //return new OkObjectResult(jsonObj);
    
    return new ContentResult
    {
      Content = jsonObj.ToJsonString(),
      ContentType = MediaTypeNames.Application.Json,
      StatusCode = StatusCodes.Status200OK
    };
  }

  private bool VerifyPositionFormat(string position)
  {
    if (string.IsNullOrEmpty(position)) return false;
    return true;
  }
}
