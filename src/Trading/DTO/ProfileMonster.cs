using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trading.DTO
{
    public class ProfileMonster
    {
        public int Count { get; set; }
        public int MatchCount { get; set; }
        public List<TradeMonster> Monsters { get; set; }
    }
}
