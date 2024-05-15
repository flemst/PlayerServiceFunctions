﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerFunctions
{
  internal interface IStorageConnector
  {
    void Add(Player player);
    Player GetById(string id);
    IEnumerable<Player> GetByPosition();
  }
}