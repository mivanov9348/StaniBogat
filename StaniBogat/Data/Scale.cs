namespace StaniBogat.Data
{
    public class Scale
    {

        public int Id { get; set; }

        public double Number { get; set; }

        public int Money { get; set; }

        public ICollection<Quest> Quests { get; set; } = new HashSet<Quest>();

    }
}
