using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerFunctions
{
  internal class InMemoryStorage : IStorageConnector
  {
    public void Add(Player player)
    {
      throw new NotImplementedException();
    }

    public Player GetById(string id)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Player> GetByPosition()
    {
      throw new NotImplementedException();
    }
  }
}
