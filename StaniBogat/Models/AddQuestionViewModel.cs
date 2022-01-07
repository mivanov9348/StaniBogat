using StaniBogat.Data;

namespace StaniBogat.Models
{
    public class AddQuestionViewModel
    {
        public string Question { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public string CorrectAnswer { get; set; }
        public int ScaleId { get; set; }

       
        public List<Quest> Questions { get; set; } = new List<Quest>();
        public List<Scale> Scales { get; set; } = new List<Scale>();
    }
}
