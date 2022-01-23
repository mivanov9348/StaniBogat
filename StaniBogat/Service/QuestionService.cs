namespace StaniBogat.Service
{

    using Microsoft.Data.SqlClient;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;
    using Newtonsoft.Json;
    using StaniBogat.Data;
    using StaniBogat.Models;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;


    public class QuestionService : IQuestionService
    {
        private readonly StaniBogatDbContext data;
        private Random rnd;

        public QuestionService(StaniBogatDbContext data)
        {
            this.data = data;
            this.rnd = new Random();
        }
        public List<Scale> AllScales() => this.data.Scales.ToList();
        public List<Quest> AllQuestions() => this.data.Quests.ToList();
        public void RegisterQuestion(AddQuestionViewModel aqvm)
        {
            var newQuestion = new Quest
            {
                Question = aqvm.Question,
                A = aqvm.A,
                B = aqvm.B,
                C = aqvm.C,
                D = aqvm.D,
                ScaleId = aqvm.ScaleId,
                CorrectAnswer = aqvm.CorrectAnswer
            };

            this.data.Quests.Add(newQuestion);
            this.data.SaveChanges();
        }
        public Game NewGame()
        {
            var newGame = new Game
            {
                QuestionOrder = 1
            };
            this.data.Games.Add(newGame);
            this.data.SaveChanges();
            return newGame;
        }
        public GameViewModel NewGameViewModel(Game game)
        {
            var question = GetQuestion(1);
            game.QuestionId = question.Id;
            var newGameViewModel = new GameViewModel
            {
                QuestionOrder = game.QuestionOrder,
                Question = question.Question,
                GameId = game.Id,
                A = question.A,
                B = question.B,
                C = question.C,
                D = question.D,
                CorrectAnswer = question.CorrectAnswer,
                Scales = AllScales()
            };
            this.data.SaveChanges();
            return newGameViewModel;
        }
        public Quest GetQuestion(int num)
        {
            var questionList = this.data.Quests.Where(x => x.Scale.Number == num).ToList();
            return questionList[rnd.Next(0, questionList.Count)];
        }
        public GameViewModel CheckAnswer(GameViewModel gvm)
        {
            var currGame = this.data.Games.FirstOrDefault(x => x.Id == gvm.GameId);
            var order = currGame.QuestionOrder;
            var currMoney = this.data.Scales.FirstOrDefault(x => x.Number == order).Money;
            var currQuestion = this.data.Quests.FirstOrDefault(x => x.Id == currGame.QuestionId);
            var correctAnswer = currQuestion.CorrectAnswer;
            var playerAnswer = gvm.PlayerAnswer.Substring(0, 1);

            if (correctAnswer == playerAnswer)
            {
                order++;
                if (order == 16)
                {
                    gvm.Win = true;
                    order = 15;
                }
                gvm.QuestionOrder = order;
                currGame.QuestionOrder = order;
                currGame.MoneyWin = currMoney;
                this.data.SaveChanges();
                return gvm;
            }
            else
            {
                gvm.QuestionOrder = 0;
                if (currMoney < 5000)
                {
                    gvm.MoneyWin = 0;
                }
                if (currMoney >= 5000 && currMoney < 50000)
                {
                    gvm.MoneyWin = 5000;
                }
                if (currMoney >= 50000)
                {
                    gvm.MoneyWin = 50000;
                }
            }
            return gvm;

        }
        public GameViewModel NextQuestion(GameViewModel gvm)
        {
            var currGame = this.data.Games.FirstOrDefault(x => x.Id == gvm.GameId);
            var newQuestion = GetQuestion(gvm.QuestionOrder);
            currGame.QuestionId = newQuestion.Id;
            gvm.Question = newQuestion.Question;
            gvm.PlayerAnswer = "";
            gvm.CorrectAnswer = newQuestion.CorrectAnswer;
            gvm = FillGameViewModel(gvm, newQuestion, currGame);
            this.data.SaveChanges();
            return gvm;
        }
        public GameViewModel Jokers(GameViewModel gvm, string joker)
        {
            var currGame = this.data.Games.FirstOrDefault(x => x.Id == gvm.GameId);
            var currQuestion = this.data.Quests.FirstOrDefault(x => x.Id == currGame.QuestionId);

            if (joker == "50/50")
            {
                var listAnswers = new List<string>() { "A", "B", "C", "D" };
                listAnswers = listAnswers.Where(x => x != gvm.CorrectAnswer).ToList();
                var correctAnswer = gvm.CorrectAnswer;
                var secondAnswer = listAnswers[rnd.Next(0, listAnswers.Count)];
                gvm = FillGameViewModel(gvm, currQuestion, currGame);
                if (correctAnswer != "A" && secondAnswer != "A")
                {
                    gvm.A = null;
                }
                if (correctAnswer != "B" && secondAnswer != "B")
                {
                    gvm.B = null;
                }
                if (correctAnswer != "C" && secondAnswer != "C")
                {
                    gvm.C = null;
                }
                if (correctAnswer != "D" && secondAnswer != "D")
                {
                    gvm.D = null;
                }
                currGame.FiftyFiftyUsed = true;
                gvm.FiftyFiftyUsed = true;
            }

            if (joker == "callfriend")
            {
                var correctAnswer = currQuestion.CorrectAnswer;
                var num = rnd.Next(0, 20);

                if (num <= 18)
                {
                    gvm.FriendAnswer = $"Мисля, че верният отговор е {correctAnswer}";
                }
                else
                {
                    gvm.FriendAnswer = "Не знам";
                }
                gvm = FillGameViewModel(gvm, currQuestion, currGame);
                currGame.CallUsed = true;
                gvm.CallUsed = true;
            }

            if (joker == "attendance")
            {
                var correctAnswer = currQuestion.CorrectAnswer;

                var num = 100;
                var nums = new List<int>() { 0, 0, 0, 0 };
                var count = -1;

                while (num > 0)
                {
                    count++;
                    if (count == 4)
                    {
                        nums[3] += num;
                        break;
                    }
                    var currNum = rnd.Next(0, num);
                    nums[count] = currNum;
                    num -= currNum;
                }

                var biggestNum = 0;
                foreach (var item in nums)
                {
                    if (item >= biggestNum)
                    {
                        biggestNum = item;
                    };
                }
                nums = nums.Where(x => x < biggestNum).ToList();

                switch (correctAnswer)
                {
                    case "A":
                        gvm.AttendanceAnsw.Add($"A: {biggestNum}%");
                        gvm.AttendanceAnsw.Add($"B: {nums[0]}%");
                        gvm.AttendanceAnsw.Add($"C: {nums[1]}%");
                        gvm.AttendanceAnsw.Add($"D: {nums[2]}%");
                        break;
                    case "B":
                        gvm.AttendanceAnsw.Add($"A: {nums[0]}%");
                        gvm.AttendanceAnsw.Add($"B: {biggestNum}%");
                        gvm.AttendanceAnsw.Add($"C: {nums[1]}%");
                        gvm.AttendanceAnsw.Add($"D: {nums[2]}%");
                        break;
                    case "C":
                        gvm.AttendanceAnsw.Add($"A: {nums[1]}%");
                        gvm.AttendanceAnsw.Add($"B: {nums[0]}%");
                        gvm.AttendanceAnsw.Add($"C: {biggestNum}%");
                        gvm.AttendanceAnsw.Add($"D: {nums[2]}%");
                        break;
                    case "D":
                        gvm.AttendanceAnsw.Add($"A: {nums[2]}%");
                        gvm.AttendanceAnsw.Add($"B: {nums[0]}%");
                        gvm.AttendanceAnsw.Add($"C: {nums[1]}%");
                        gvm.AttendanceAnsw.Add($"D: {biggestNum}%");
                        break;
                    default:
                        break;
                }

                gvm = FillGameViewModel(gvm, currQuestion, currGame);
                currGame.AttendanceUsed = true;
                gvm.AttendanceUsed = true;
            }

            this.data.SaveChanges();
            gvm.IsJoker = true;
            return gvm;
        }
        public GameViewModel FillGameViewModel(GameViewModel gvm, Quest question, Game currgame)
        {
            gvm.A = question.A;
            gvm.B = question.B;
            gvm.C = question.C;
            gvm.D = question.D;
            gvm.Question = question.Question;
            gvm.FiftyFiftyUsed = currgame.FiftyFiftyUsed;
            gvm.AttendanceUsed = currgame.AttendanceUsed;
            gvm.CallUsed = currgame.CallUsed;
            gvm.Scales = AllScales();
            gvm.Questions = AllQuestions();
            return gvm;
        }

    }

}
