using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlayerFunctions
{
  internal class InMemoryStorage : IStorageConnector
  {
    private static List<Player> _players = new();

    public InMemoryStorage()
    {
      _players.Add(new Player { Id = "1", Name = "Jens Jensen", Position = "1" });
    }

    public void Add(Player player)
    {
      _players.Add(player);
    }

    public Player GetById(string id)
    {
      return _players.Find(p => p.Id == id);
    }

    public IEnumerable<PositionPlayer> GetByPosition(string position)
    {
      return _players
        .Where(p => p.Position == position)
        .Select(x => new PositionPlayer() { Id = x.Id, Name = x.Name });
    }
  }
}
