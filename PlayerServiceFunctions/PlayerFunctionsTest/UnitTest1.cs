using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PlayerFunctions;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace PlayerFunctionsTest;

public class Tests
{
  private RestApi _restApi;
  private IStorageConnector _storageMock;
  private ILogger<RestApi> _loggerMock;

  //[SetUp]
  public void Setup()
  {
    _storageMock = Substitute.For<IStorageConnector>();
    _loggerMock = Substitute.For<ILogger<RestApi>>();
    _restApi = new RestApi(_loggerMock);
    //_restApi.SetStorageConnector(_storageMock);
  }

  [Test]
  public async Task AddPlayer_WithValidPlayer_ReturnsOkObjectResult()
  {
    ILogger<RestApi> loggerMock = Substitute.For<ILogger<RestApi>>();
    RestApi restApi = new RestApi(loggerMock);
    // Arrange
    var playerName = "TestPlayer";
    var playerId = "1";
    Player player = new Player { Name = playerName, Id = playerId };
    string json = JsonSerializer.Serialize(player);
    var request = new DefaultHttpContext().Request;
    request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{\"playerID\":\"user-001\",\"playerName\":\"{playerName}\",\"groupName\":\"grp01\",\"region\":\"AARHUS\",\"positionAsString\":\"(0,0,0)\",\"accessToken\":\"token#0\"}"));

    // Act
    var result = await restApi.AddPlayer(request);

    // Assert
    // Assert.IsInstanceOf<OkObjectResult>(result);
    var okResult = (OkObjectResult)result;
    var addedPlayer = (Player)okResult.Value;
    addedPlayer.Name.Should().Be(playerName);
    addedPlayer.Id.Should().Be(playerId);
  }
  //  [Test]
  //  public async Task GetPlayerById_WithExistingPlayer_ReturnsOkObjectResult()
  //  {
  //    // Arrange
  //    var playerId = "testId";
  //    var existingPlayer = new Player { Name = "TestPlayer", Id = playerId };
  //    _storageMock.GetById(playerId).Returns(existingPlayer);

  //    // Act
  //    var result = _restApi.GetPlayerById(null, playerId);

  //    // Assert
  //    Assert.IsInstanceOf<OkObjectResult>(result);
  //    var okResult = (OkObjectResult)result;
  //    var playerJsonString = (string)okResult.Value;
  //    var deserializedPlayer = JsonSerializer.Deserialize<Player>(playerJsonString);
  //    Assert.AreEqual(existingPlayer.Name, deserializedPlayer.Name);
  //    Assert.AreEqual(existingPlayer.Id, deserializedPlayer.Id);
  //  }

  //  [Test]
  //  public void GetPlayerById_WithNonExistingPlayer_ReturnsNotFoundObjectResult()
  //  {
  //    // Arrange
  //    var playerId = "nonExistingId";
  //    _storageMock.GetById(playerId).Returns((Player)null);

  //    // Act
  //    var result = _restApi.GetPlayerById(null, playerId);

  //    // Assert
  //    Assert.IsInstanceOf<NotFoundObjectResult>(result);
  //  }

  //  [Test]
  //  public void GetPlayersByPosition_WithValidPosition_ReturnsOkObjectResult()
  //  {
  //    // Arrange
  //    var position = "goalkeeper";
  //    var playersAtPosition = new List<PositionPlayer>
  //          {
  //              new PositionPlayer { Id = "1", Name = "Player1" },
  //              new PositionPlayer { Id = "2", Name = "Player2" }
  //          };
  //    _storageMock.GetByPosition(position).Returns(playersAtPosition);

  //    // Act
  //    var result = _restApi.GetPlayersByPosition(null, position);

  //    // Assert
  //    Assert.IsInstanceOf<OkObjectResult>(result);
  //    var okResult = (OkObjectResult)result;
  //    var jsonResponse = (JsonObject)okResult.Value;
  //    var playersArray = (JsonArray)jsonResponse["players"];
  //    Assert.AreEqual(2, playersArray.Count);
  //    // Assert other conditions as needed
  //  }

  //  // Add more tests as needed for edge cases, error handling, etc.
  //}
}