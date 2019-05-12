using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trading.Models;

namespace Trading.DTO
{
    public class TradingDetails
    {
        public List<UserMonster> MatchingSearch { get; set; }
        public List<UserMonster> MatchingPropose { get; set; }
    }
}
