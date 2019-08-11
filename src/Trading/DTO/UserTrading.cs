using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trading.DTO
{
    public class UserTrading
    {
        public IEnumerable<TradeMonster> MatchTargetSearch { get; set; }
        public IEnumerable<TradeMonster> MatchUserSearch { get; set; }
        public IEnumerable<TradeMonster> TargetSearch { get; set; }
        public IEnumerable<TradeMonster> UserSearch { get; set; }
    }
}
