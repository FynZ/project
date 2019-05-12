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
    }
}
