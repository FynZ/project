using System.Collections.Generic;
using System.Linq;
using Trading.DTO;
using Trading.Repositories;
using Trading.Utils;

namespace Trading.Services
{
    public class TradingService : ITradingService
    {
        private readonly ITradingRepository _tradingRepository;

        public TradingService(ITradingRepository tradingRepository)
        {
            _tradingRepository = tradingRepository;
        }

        public IEnumerable<TradingResult> GetTradableUsers(int userId)
        {
            var monsters = _tradingRepository.GetMonsters().ToList();

            var userMonsters = monsters.Where(x => x.UserId == userId).ToList();
            monsters.RemoveAll(x => x.UserId == userId);

            var dict = monsters.GroupBy(x => x.UserId).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

            var proposeIds = userMonsters.Where(x => x.Propose).Select(x => x.Id).ToList();
            var searchIds = userMonsters.Where(x => x.Search).Select(x => x.Id).ToList();

            var result = new List<TradingResult>();
            foreach (var user in dict)
            {
                var proposed = user.Value.Where(x => x.Propose && searchIds.Contains(x.Id)).ToList();
                var searched = user.Value.Where(x => x.Search && proposeIds.Contains(x.Id)).ToList();

                result.Add(new TradingResult
                {
                    UserId = user.Key,
                    Affinity = TradingUtils.GetAffinity(searched.Count, proposed.Count),
                    MatchingSearchCount = searched.Count,
                    MatchingProposeCount = proposed.Count
                });
            }

            return result;
        }

        public TradingDetails GetTradingDetails(int userId, int targetId)
        {
            var userMonsters = _tradingRepository.GetUserMonsters(userId).ToList();
            var targetMonsters = _tradingRepository.GetUserMonsters(targetId).ToList();

            var searchIdsPlayer = userMonsters.Where(x => x.Search).Select(x => x.Id).ToArray();
            var searchIdsTarget = targetMonsters.Where(x => x.Search).Select(x => x.Id).ToArray();

            var propose = userMonsters.Where(x => x.Propose && searchIdsTarget.Contains(x.Id)).ToList();
            var search = targetMonsters.Where(x => x.Propose && searchIdsPlayer.Contains(x.Id)).ToList();

            return new TradingDetails
            {
                MatchingPropose = propose,
                MatchingSearch = search
            };
        }
    }
}
