using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PlayerFunctions.EndPoints
{
  public class AddPlayerFunction
  {
    private ILogger<AddPlayerFunction> _logger;
    private IStorageConnector _storage;

    public AddPlayerFunction(ILogger<AddPlayerFunction> logger, IStorageConnector storage)
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
      if (player.Id == null)
        return new BadRequestResult();

      // Add player to storage
      _logger.LogInformation($"Adding player to storage \n" + player);
      _storage.AddOrUpdate(player);

      return new OkResult();
    }
  }
}
