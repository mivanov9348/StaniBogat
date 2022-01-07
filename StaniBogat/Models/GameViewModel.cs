using StaniBogat.Data;

namespace StaniBogat.Models
{
    public class GameViewModel
    {

        public string Question { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public int QuestionOrder { get; set; }

        public string PlayerAnswer { get; set; }

        public string CorrectAnswer { get; set; }

        public string FriendAnswer { get; set; }

        public List<string> AttendanceAnsw { get; set; } = new List<string>();

        public int GameId { get; set; }

        public int MoneyWin { get; set; } = -1;

        public bool FiftyFiftyUsed { get; set; }

        public bool CallUsed { get; set; }

        public bool AttendanceUsed { get; set; }

        public bool Win { get; set; }

        public bool IsJoker { get; set; }       

        public List<Quest> Questions { get; set; } = new List<Quest>();
        public List<Scale> Scales { get; set; } = new List<Scale>();




    }
}
