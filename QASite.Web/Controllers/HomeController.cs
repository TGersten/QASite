using Microsoft.AspNetCore.Mvc;
using QASite.Data;
using QASite.Web.Models;
using System.Diagnostics;

namespace QASite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new QuestionsRepository(_connectionString);
            var vm = new IndexViewModel();
            vm.Questions = repo.GetAllQuestions();

            return View(vm);
        }

       
    }
}
