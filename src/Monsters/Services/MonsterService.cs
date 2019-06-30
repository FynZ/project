using System.Collections.Generic;
using System.Linq;
using Monsters.DTO;
using Monsters.Models;
using Monsters.Repositories;

namespace Monsters.Services
{
    public class MonsterService : IMonsterService
    {
        private readonly IMonsterRepository _monsterRepository;

        public MonsterService(IMonsterRepository monsterRepository)
        {
            _monsterRepository = monsterRepository;
        }

        public IEnumerable<UserMonster> GetUserMonsters(int userId)
        {
            return _monsterRepository.GetUserMonsters(userId);
        }

        public UserMonster GetUserMonster(int monsterId, int userId)
        {
            return _monsterRepository.GetUserMonster(monsterId, userId);
        }

        public MonsterSummary GetSummary(int userId)
        {
            var monsters = _monsterRepository.GetUserMonsters(userId).ToList();

            return new MonsterSummary
            {
                HaveCount = monsters.Count(x => x.Count > 0),
                ProposeCount = monsters.Count(x => x.Propose),
                SearchCount = monsters.Count(x => x.Search),
                TradableCount = monsters.Count(x => x.Count > 1)
            };
        }

        public IEnumerable<UserMonster> GetProposedMonsters(int userId)
        {
            var monsters = _monsterRepository.GetUserMonsters(userId);

            return monsters.Where(x => x.Propose);
        }

        public IEnumerable<UserMonster> GetSearchedMonsters(int userId)
        {
            var monsters = _monsterRepository.GetUserMonsters(userId);

            return monsters.Where(x => x.Search);
        }
    }
}
