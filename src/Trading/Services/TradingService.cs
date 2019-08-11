using System.Collections.Generic;
using System.Linq;
using Trading.DTO;
using Trading.Models;
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

        public ProfileMonster GetSearchedMonstersWithMatchs(int userId, int targetId)
        {
            var userMonsters = _tradingRepository.GetUserMonsters(userId).ToDictionary(x => x.Id);
            var targetMonsters = _tradingRepository.GetUserMonsters(targetId).ToList();

            var monsters = targetMonsters.Where(x => x.Search).Select(x => new TradeMonster
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                MinLevel = x.MinLevel,
                MaxLevel = x.MaxLevel,
                AnkamaId = x.AnkamaId,
                //Match = userMonsters.TryGetValue(x.Id, out var monster) ? monster.Propose ? true : false : false
            })/*.OrderByDescending(x => x.Match)*/.ToList();

            return new ProfileMonster
            {
                Count = monsters.Count,
                //MatchCount = monsters.Count(x => x.Match),
                Monsters = monsters
            };
        }

        public ProfileMonster GetProposedMonstersWithMatchs(int userId, int targetId)
        {
            var userMonsters = _tradingRepository.GetUserMonsters(userId).ToList();
            var targetMonsters = _tradingRepository.GetUserMonsters(targetId).ToDictionary(x => x.Id);

            var monsters = userMonsters.Where(x => x.Search).Select(x => new TradeMonster
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                MinLevel = x.MinLevel,
                MaxLevel = x.MaxLevel,
                AnkamaId = x.AnkamaId,
                //Match = targetMonsters.TryGetValue(x.Id, out var monster) ? monster.Propose ? true : false : false
            })/*.OrderByDescending(x => x.Match)*/.ToList();

            return new ProfileMonster
            {
                //Count = monsters.Count,
                //MatchCount = monsters.Count(x => x.Match),
                Monsters = monsters
            };
        }

        public UserTrading GetDetailTrade(int userId, int targetId)
        {
            var userMonsters = _tradingRepository.GetUserMonsters(userId).ToDictionary(x => x.Id);
            var targetMonsters = _tradingRepository.GetUserMonsters(targetId).ToDictionary(x => x.Id);

            var matchUser = new List<TradeMonster>();
            var nomatchUser = new List<TradeMonster>();
            foreach (var monster in userMonsters.Values)
            {
                if (targetMonsters.TryGetValue(monster.Id, out var targetMonster))
                {
                    if (targetMonster.Search && monster.Propose)
                    {
                        matchUser.Add(this.UserMonsterToTradeMonster(monster));
                    }
                    else if (monster.Search)
                    {
                        nomatchUser.Add(this.UserMonsterToTradeMonster(monster));
                    }
                }
            }

            var matchTarget = new List<TradeMonster>();
            var nomatchTarget = new List<TradeMonster>();
            foreach (var monster in targetMonsters.Values)
            {
                if (userMonsters.TryGetValue(monster.Id, out var userMonster))
                {
                    if (userMonster.Search && monster.Propose)
                    {
                        matchTarget.Add(this.UserMonsterToTradeMonster(monster));
                    }
                    else if (monster.Search)
                    {
                        nomatchTarget.Add(this.UserMonsterToTradeMonster(monster));
                    }
                }
            }

            return new UserTrading
            {
                MatchTargetSearch = matchTarget,
                TargetSearch = nomatchTarget,
                MatchUserSearch = matchUser,
                UserSearch = nomatchUser
            };
        }

        private TradeMonster UserMonsterToTradeMonster(UserMonster monster)
        {
            return new TradeMonster
            {
                Id = monster.Id,
                Name = monster.Name,
                Slug = monster.Slug,
                MinLevel = monster.MinLevel,
                MaxLevel = monster.MaxLevel,
                AnkamaId = monster.AnkamaId,
            };
        }
    }
}
