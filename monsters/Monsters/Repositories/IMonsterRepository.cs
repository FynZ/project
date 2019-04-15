using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Models;

namespace Accounts.Repositories
{
    public interface IMonsterRepository
    {
        bool InitUser(int userId);
        UserMonster GetUserMonster(int monsterId, int userId);
        IEnumerable<UserMonster> GetUserMonsters(int userId);
        void IncrementMonster(int monsterId, int userId);
        void DecrementMonster(int monsterId, int userId);
        void SearchMonster(int monsterId, int userId);
        void UnsearchMonster(int monsterId, int userId);
        void ProposeMonster(int monsterId, int userId);
        void UnproposeMonster(int monsterId, int userId);
    }
}
