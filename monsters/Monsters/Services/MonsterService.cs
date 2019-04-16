using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Models;
using Accounts.Repositories;

namespace Accounts.Services
{
    public class MonsterService : IMonsterService
    {
        private readonly IMonsterRepository _monsterRepository;

        public MonsterService(IMonsterRepository monsterRepository)
        {
            _monsterRepository = monsterRepository;
        }
        public bool InitUser(int userId)
        {
            if (this.IsUserInited(userId))
            {
                return false;
            }

            return _monsterRepository.InitUser(userId);
        }

        public UserMonster GetUserMonster(int monsterId, int userId)
        {
            return _monsterRepository.GetUserMonster(monsterId, userId);
        }

        public IEnumerable<UserMonster> GetUserMonsters(int userId)
        {
            return _monsterRepository.GetUserMonsters(userId);
        }

        public void IncrementMonster(int monsterId, int userId)
        {
            _monsterRepository.IncrementMonster(monsterId, userId);
        }

        public void DecrementMonster(int monsterId, int userId)
        {
            var userMonster = _monsterRepository.GetUserMonster(monsterId, userId);

            if (userMonster.Count > 0)
            {
                _monsterRepository.DecrementMonster(monsterId, userId);
            }
        }

        public void SearchMonster(int monsterId, int userId)
        {
            _monsterRepository.SearchMonster(monsterId, userId);
        }

        public void UnsearchMonster(int monsterId, int userId)
        {
            _monsterRepository.UnsearchMonster(monsterId, userId);
        }

        public void ProposeMonster(int monsterId, int userId)
        {
            _monsterRepository.ProposeMonster(monsterId, userId);
        }

        public void UnproposeMonster(int monsterId, int userId)
        {
            _monsterRepository.UnproposeMonster(monsterId, userId);
        }

        private bool IsUserInited(int userId)
        {
            if (_monsterRepository.GetUserMonster(1, userId) != null)
            {
                return false;
            }

            return true;
        }
    }
}
