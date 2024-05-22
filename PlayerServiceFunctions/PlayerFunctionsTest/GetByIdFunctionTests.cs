using Microsoft.AspNetCore.Mvc;
using PlayerFunctions.EndPoints;
using PlayerFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PlayerFunctionsTest;

public class GetByIdFunctionTests
{
  private GetByIdFunction _getByIdFunction;
  private IStorageConnector _storageMock;
  private ILogger<GetByIdFunction> _loggerMock;

  [SetUp]
  public void Setup()
  {
    _storageMock = Substitute.For<IStorageConnector>();
    _loggerMock = Substitute.For<ILogger<GetByIdFunction>>();
    _getByIdFunction = new GetByIdFunction(_loggerMock, _storageMock);
  }

  [Test]
  public async Task GetPlayerById_WithExistingPlayerId_ReturnsCorrectContent()
  {
    // Arrange
    var playerId = "1";
    string playerName = "Player";
    var existingPlayer = new Player { Name = playerName, Id = playerId };
    _storageMock.GetById(playerId).Returns(existingPlayer);

    // Act
    var result = _getByIdFunction.GetPlayerById(null, playerId);

    // Assert
    var contentResult = (ContentResult)result;
    contentResult.ContentType.Should().Be("application/json");
    contentResult.StatusCode.Should().Be(StatusCodes.Status200OK);

    var deserializedPlayer = JsonSerializer.Deserialize<Player>(contentResult.Content);
    deserializedPlayer.Id.Should().Be(playerId);
    deserializedPlayer.Name.Should().Be(playerName);
  }

  [Test]
  public void GetPlayerById_WithNonExistingPlayer_ReturnsNotFoundStatus()
  {
    // Act
    var result = _getByIdFunction.GetPlayerById(null, "2");

    // Assert
    ((IStatusCodeActionResult)result).StatusCode
      .Should().Be(StatusCodes.Status404NotFound);
  }
}
