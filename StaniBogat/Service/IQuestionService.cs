namespace StaniBogat.Service
{
    using StaniBogat.Data;
    using StaniBogat.Models;

    public interface IQuestionService
    {

        List<Scale> AllScales();
        List<Quest> AllQuestions();      
        void RegisterQuestion(AddQuestionViewModel aqvm);
        Game NewGame();
        GameViewModel NewGameViewModel(Game game);
        Quest GetQuestion(int num);
        GameViewModel NextQuestion(GameViewModel gvm);
        GameViewModel CheckAnswer(GameViewModel gvm);
        GameViewModel Jokers(GameViewModel gvm, string joker);
    }
}
