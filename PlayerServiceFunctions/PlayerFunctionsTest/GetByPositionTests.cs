using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PlayerFunctions.EndPoints;
using PlayerFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Nodes;

namespace PlayerFunctionsTest
{
  public class GetByPositionTests
  {
    private GetByPositionFunction _getByPositionFunction;
    private IStorageConnector _storageMock;
    private ILogger<GetByPositionFunction> _loggerMock;

    [SetUp]
    public void Setup()
    {
      _storageMock = Substitute.For<IStorageConnector>();
      _loggerMock = Substitute.For<ILogger<GetByPositionFunction>>();
      _getByPositionFunction = new GetByPositionFunction(_loggerMock, _storageMock);
    }

    [Test]
    public void GetPlayersByPosition_WithValidPosition_ReturnsCorrectContent()
    {
      // Arrange
      string position = "(1,1,1)";
      string id1 = "1";
      string name2 = "Player2";
      List<Player> playersAtPosition = 
        new List<Player>
        {
          new Player { Id = id1, Name = "Player1" },
          new Player { Id = "2", Name = name2 }
        };
      _storageMock.GetByPosition(position).Returns(playersAtPosition);

      // Act
      var result = _getByPositionFunction.GetPlayersByPosition(null, position);

      // Assert
      var contentResult = (ContentResult)result;
      contentResult.ContentType.Should().Be("application/json");
      contentResult.StatusCode.Should().Be(StatusCodes.Status200OK);

      JsonArray playerArray = (JsonArray)JsonNode.Parse(contentResult?.Content)["players"];
      playerArray[0]["playerID"].ToString().Should().Be(id1);
      playerArray[1]["playerName"].ToString().Should().Be(name2);
    }

    [Test]
    public void GetPlayersByPosition_WithInValidPosition_ReturnsBadRequestCode()
    {
      // Act
      var result = _getByPositionFunction.GetPlayersByPosition(null, "1,1,1");

      // Assert
      result.GetType().Should().Be(typeof(BadRequestResult));
    }
  }
}
