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
    }
}
