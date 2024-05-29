namespace PlayerFunctions
{
  public class InMemoryStorage : IStorageConnector
  {
    private static Dictionary<string, Player> _players = new();

    public void AddOrUpdate(Player player)
    {
      if (_players.ContainsKey(player.Id))
        _players[player.Id] = player;
      else
        _players.Add(player.Id, player);
    }

    public Player GetById(string id)
    {
      if (_players.ContainsKey(id))
        return _players[id];
      return null;
    }

    public IEnumerable<Player> GetByPosition(string position)
    {
      return _players
        .Where(p => p.Value.Position == position)
        .Select(x => x.Value);
    }
  }
}
