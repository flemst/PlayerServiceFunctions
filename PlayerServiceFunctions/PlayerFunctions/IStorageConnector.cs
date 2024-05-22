using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerFunctions
{
  public interface IStorageConnector
  {
    void AddOrUpdate(Player player);
    Player GetById(string id);
    IEnumerable<Player> GetByPosition(string position);
  }
}
