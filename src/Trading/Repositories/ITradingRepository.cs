using System.Collections.Generic;
using Trading.Models;

namespace Trading.Repositories
{
    public interface ITradingRepository
    {
        IEnumerable<UserMonster> GetMonsters();

        IEnumerable<UserMonster> GetUserMonsters(int userId);
    }
}
