namespace StaniBogat.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StaniBogat.Models;
    using StaniBogat.Service;
    using System.Diagnostics;
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuestionService questionservice;
        public HomeController(ILogger<HomeController> logger, IQuestionService questionservice)
        {
            _logger = logger;
            this.questionservice = questionservice;
        }
        public IActionResult HomeScreen(GameViewModel gvm)
        {        
            return View(gvm);
        }
        public IActionResult Index(GameViewModel gvm)
        {
            if (gvm.IsJoker == true)
            {
                gvm.Scales = questionservice.AllScales();
                return View(gvm);
            }
            if (gvm == null || gvm.QuestionOrder < 1)
            {
                var game = questionservice.NewGame();
                gvm = questionservice.NewGameViewModel(game);
            }
            else
            {
                gvm = questionservice.NextQuestion(gvm);
            }

            return View(gvm);
        }

        [HttpPost]
        public IActionResult SelectAnswer(GameViewModel gvm)
        {
            gvm = questionservice.CheckAnswer(gvm);
            Thread.Sleep(2500);
            if (gvm.QuestionOrder == 0 || gvm.Win == true)
            {
                if (gvm.Win == true)
                {
                    gvm.MoneyWin = 1000000;
                }
                return View("HomeScreen", gvm);
            }
            return RedirectToAction("Index", gvm);
        }

        public IActionResult AddQuestion()
        {
            return View(new AddQuestionViewModel
            {
                Scales = questionservice.AllScales(),
                Questions = questionservice.AllQuestions()
            });
        }
        public IActionResult RegisterQuestion(AddQuestionViewModel aqvm)
        {
            questionservice.RegisterQuestion(aqvm);
            return RedirectToAction("AddQuestion");
        }
        public IActionResult FiftyFifty(GameViewModel gvm)
        {
            gvm = questionservice.Jokers(gvm, "50/50");
            return RedirectToAction("Index", gvm);
        }
        public IActionResult CallFriend(GameViewModel gvm)
        {
            gvm = questionservice.Jokers(gvm, "callfriend");
            return RedirectToAction("Index", gvm);
        }

        public IActionResult Attendance(GameViewModel gvm)
        {
            gvm = questionservice.Jokers(gvm, "attendance");
            return RedirectToAction("Index", gvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}