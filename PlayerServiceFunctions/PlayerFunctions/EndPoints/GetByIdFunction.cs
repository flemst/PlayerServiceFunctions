using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Text.Json;

namespace PlayerFunctions.EndPoints
{
  public class GetByIdFunction
  {
    private ILogger<GetByIdFunction> _logger;
    private IStorageConnector _storage;

    public GetByIdFunction(ILogger<GetByIdFunction> logger, IStorageConnector storage)
    {
      _logger = logger;
      _storage = storage;
    }

    [Function("GetPlayerById")]
    public IActionResult GetPlayerById(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get",
    Route = "v1/players/{id}")] HttpRequest req, string id)
    {
      Player player = _storage.GetById(id);

      // When player doesn't exist
      if (player == null)
        return new NotFoundObjectResult(string.Empty);

      _logger.LogInformation($"Found player \n" + player);

      return new ContentResult
      {
        Content = JsonSerializer.Serialize(player),
        ContentType = MediaTypeNames.Application.Json,
        StatusCode = StatusCodes.Status200OK
      };
    }
  }
}
