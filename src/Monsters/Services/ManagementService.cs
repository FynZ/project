using Monsters.Repositories;

namespace Monsters.Services
{
    public class ManagementService : IManagementService, IMonsterIniter
    {
        private readonly IMonsterRepository _monsterRepository;

        public ManagementService(IMonsterRepository monsterRepository)
        {
            _monsterRepository = monsterRepository;
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

        public bool InitUser(int userId)
        {
            if (this.IsUserInited(userId))
            {
                return false;
            }

            return _monsterRepository.InitUser(userId);
        }

        private bool IsUserInited(int userId)
        {
            var monsters = _monsterRepository.GetUserMonster(1, userId);

            return monsters != null;
        }
    }
}
