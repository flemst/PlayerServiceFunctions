using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Text.Json.Nodes;

namespace PlayerFunctions.EndPoints
{
  public class GetByPositionFunction
  {
    private ILogger<GetByPositionFunction> _logger;
    private IStorageConnector _storage;

    public GetByPositionFunction(ILogger<GetByPositionFunction> logger, IStorageConnector storage)
    {
      _logger = logger;
      _storage = storage;
    }

    [Function("GetPlayersByPosition")]
    public IActionResult GetPlayersByPosition(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get",
    Route = "v1/players/position/{position}")] HttpRequest req, string position)
    {
      if (!ValidatePositionFormat(position))
        return new BadRequestResult();

      IEnumerable<Player> playersAtPosition = _storage.GetByPosition(position);

      // Build response JSON object
      JsonArray jsonArray = new();
      foreach (Player player in playersAtPosition)
      {
        JsonObject jsonPlayer = 
          new()
          {
            { "playerID", player.Id },
            { "playerName", player.Name }
          };
        jsonArray.Add(jsonPlayer);
      }
      JsonObject jsonObj = 
        new JsonObject
        {
          { "players", jsonArray }
        };

      return new ContentResult
      {
        Content = jsonObj.ToJsonString(),
        ContentType = MediaTypeNames.Application.Json,
        StatusCode = StatusCodes.Status200OK
      };
    }

    private bool ValidatePositionFormat(string position)
    {
      return 
        !string.IsNullOrEmpty(position) &&
        position[0] == '(' && 
        position[position.Length - 1] == ')';
    }
  }
}
