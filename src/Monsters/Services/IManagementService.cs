namespace Monsters.Services
{
    public interface IManagementService
    {
        void IncrementMonster(int monsterId, int userId);
        void DecrementMonster(int monsterId, int userId);
        void SearchMonster(int monsterId, int userId);
        void UnsearchMonster(int monsterId, int userId);
        void ProposeMonster(int monsterId, int userId);
        void UnproposeMonster(int monsterId, int userId);
    }
}
