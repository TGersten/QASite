using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QASite.Data;
using QASite.Web.Models;

namespace QASite.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly string _connectionString;
        public QuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Authorize]
        public IActionResult AskAQuestion()
        {
           
                return View(); 
            
          
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(Question question, List<string> tags)
        {
            question.DatePosted = DateTime.Now;
         
            question.UserId = GetCurrentUserId().Value;
            var repo = new QuestionsRepository(_connectionString);
            repo.AddQuestion(question, tags);
            return Redirect("/home/index");
        }

        public IActionResult ViewQuestion(int id)
        {
            var repo = new QuestionsRepository(_connectionString);
            var vm = new ViewQuestionViewModel
            {
                Question = repo.GetQuestionForId(id)
            };

            if (vm.Question==null)
            {
                return Redirect("/home/index");
            }

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer answer)
        {
            answer.DatePosted = DateTime.Now;
            answer.UserId = GetCurrentUserId().Value;
            var repo = new QuestionsRepository(_connectionString);
            repo.AddAnswer(answer);

            return Redirect($"/question/viewQuestion?id={answer.QuestionId}");
        }

        public IActionResult ViewQuestionsForTag(string name)
        {
            var repo = new QuestionsRepository(_connectionString);
            var vm = new ViewQuestionsForTagViewModel();
            vm.Questions = repo.GetQuestionsForTag(name);

            return View(vm);
        }

        private int? GetCurrentUserId()
        {
            var repo = new UserRepository(_connectionString);
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            else
            {
                var user = repo.GetUserByEmail(User.Identity.Name);
                if (user == null)
                {
                    return null;
                }

                return user.Id;

            }
        }
    }
}
