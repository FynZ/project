using System.Collections.Generic;
using Monsters.DTO;
using Monsters.Models;

namespace Monsters.Services
{
    public interface IMonsterService
    {
        IEnumerable<UserMonster> GetUserMonsters(int userId);
        MonsterSummary GetSummary(int userId);
        IEnumerable<UserMonster> GetSearchedMonsters(int userId);
        IEnumerable<UserMonster> GetProposedMonsters(int userId);
        void IncrementMonster(int monsterId, int userId);
        void DecrementMonster(int monsterId, int userId);
        void SearchMonster(int monsterId, int userId);
        void UnsearchMonster(int monsterId, int userId);
        void ProposeMonster(int monsterId, int userId);
        void UnproposeMonster(int monsterId, int userId);
    }
}
