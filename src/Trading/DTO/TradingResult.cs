using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trading.Models;

namespace Trading.DTO
{
    public class TradingResult
    {
        public int UserId { get; set; }

        public int MatchingSearchCount { get; set; }

        public int MatchingProposeCount { get; set; }
        //public List<UserMonster> Search { get; set; }
        //public List<UserMonster> Propose { get; set; }

        public TradingResult(int userId, int matchingSearchCount, int matchingProposeCount)
        {
            UserId = userId;
            MatchingProposeCount = matchingProposeCount;
            MatchingSearchCount = matchingSearchCount;
        }
    }
}
