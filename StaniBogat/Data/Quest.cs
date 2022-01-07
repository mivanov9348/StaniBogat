namespace StaniBogat.Data
{
    public class Quest
    {

        public int Id { get; set; }

        public string Question  { get; set; }

        public string A  { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public string CorrectAnswer { get; set; }

        public Scale Scale { get; set; }
        public int ScaleId { get; set; }

    }
}
