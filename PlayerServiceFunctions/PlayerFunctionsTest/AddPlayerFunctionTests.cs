using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PlayerFunctions;
using System.Text;
using System.Text.Json;
using PlayerFunctions.EndPoints;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PlayerFunctionsTest;

public class AddPlayerFunctionTests
{
  private AddPlayerFunction _addPlayerFunction;
  private IStorageConnector _storageMock;
  private ILogger<AddPlayerFunction> _loggerMock;

  [SetUp]
  public void Setup()
  {
    _storageMock = Substitute.For<IStorageConnector>();
    _loggerMock = Substitute.For<ILogger<AddPlayerFunction>>();
    _addPlayerFunction = new AddPlayerFunction(_loggerMock, _storageMock);
  }

  [Test]
  public async Task AddPlayer_WithValidJsonBody_ReturnsOkStatus()
  {
    // Arrange
    string playerName = "Player";
    string playerId = "1";
    Player player = new Player { Name = playerName, Id = playerId };
    string jsonPlayer = JsonSerializer.Serialize(player);
    HttpRequest request = new DefaultHttpContext().Request;
    request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonPlayer));

    // Act
    var result = await _addPlayerFunction.AddPlayer(request);

    // Assert
    ((IStatusCodeActionResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
  }

  [Test]
  public async Task AddPlayer_WithInvalidJsonBody_ReturnsBadRequestStatus()
  {
    // Arrange
    HttpRequest request = new DefaultHttpContext().Request;
    string s = "{\"invalidID\":\"1\",\"ivalidName\":\"Player\"}";
    request.Body = new MemoryStream(Encoding.UTF8.GetBytes(s));

    // Act
    var result = await _addPlayerFunction.AddPlayer(request);

    // Assert
    ((IStatusCodeActionResult)result).StatusCode.Should().Be(StatusCodes.Status400BadRequest);
  }
}