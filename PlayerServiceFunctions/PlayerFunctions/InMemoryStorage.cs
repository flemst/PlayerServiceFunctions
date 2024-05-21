using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlayerFunctions
{
  public class InMemoryStorage : IStorageConnector
  {
    //private static List<Player> _players = new();
    private static Dictionary<string, Player> _playersDict = new();

    public void AddOrUpdate(Player player)
    {
      Player p;
      var test = _playersDict.TryGetValue("", out p);


      //_players.Add(player);
      
      if (_playersDict.ContainsKey(player.Id))
        _playersDict[player.Id] = player;
      else
        _playersDict.Add(player.Id, player);
    }

    public Player GetById(string id)
    {
      if (_playersDict.ContainsKey(id))
        return _playersDict[id];
      return null;

      //return _players.Find(p => p.Id == id);
      //return _playersDict[id];
    }

    public IEnumerable<PositionPlayer> GetByPosition(string position)
    {
      //return _players
      //  .Where(p => p.Position == position)
      //  .Select(x => new PositionPlayer() { Id = x.Id, Name = x.Name });

      return _playersDict
        .Where(p => p.Value.Position == position)
        .Select(x => 
          new PositionPlayer() 
          { 
            Id = x.Value.Id,
            Name = x.Value.Name
          });
    }
  }
}
