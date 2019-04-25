namespace Monsters.Models
{
    public class UserMonster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int AnkamaId { get; set; }
        public int Count { get; set; }
        public bool Search { get; set; }
        public bool Propose { get; set; }
        public int UserId { get; set; }
    }
}
