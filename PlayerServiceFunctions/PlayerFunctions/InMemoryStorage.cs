namespace PlayerFunctions
{
  public class InMemoryStorage : IStorageConnector
  {
    private static Dictionary<string, Player> _playersDict = new();

    public void AddOrUpdate(Player player)
    {
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
    }

    public IEnumerable<Player> GetByPosition(string position)
    {
      return _playersDict
        .Where(p => p.Value.Position == position)
        .Select(x => x.Value);
    }
  }
}
