using System.Collections.Generic;
using Trading.DTO;

namespace Trading.Services
{
    public interface ITradingService
    {
        IEnumerable<TradingResult> GetTradableUsers(int userId);
        TradingDetails GetTradingDetails(int userId, int targetId);
        ProfileMonster GetSearchedMonstersWithMatchs(int userId, int targetId);
        ProfileMonster GetProposedMonstersWithMatchs(int userId, int targetId);
        UserTrading GetDetailTrade(int userId, int targetId);
    }
}
