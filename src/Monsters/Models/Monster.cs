namespace Monsters.Models
{
    public class Monster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int AnkamaId { get; set; }
    }
}
