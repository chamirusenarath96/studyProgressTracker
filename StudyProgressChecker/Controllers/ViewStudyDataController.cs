using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Xml.Linq;

namespace StudyProgressChecker.Controllers
{
    public class ViewStudyDataController : Controller

    {
        private readonly ILogger<HomeController> _logger;

        public ViewStudyDataController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /StudyProgressBars
        public IActionResult Index()
        {
            List<string> stringList = new List<string>
            {
                "Apple",
                "Banana",
                "Orange"
            };

            //return "This is test controller action for view all study progresses";
            ViewData["studySessions"] = stringList;
            return View();
        }

        // GET: /StudyProgressBars/ViewUserProgress
        public IActionResult ViewUserProgress(string name)
        {
            ViewData["name"] = name;
            return View();
        }




    }
}
