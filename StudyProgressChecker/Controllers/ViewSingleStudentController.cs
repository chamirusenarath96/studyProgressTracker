using Microsoft.AspNetCore.Mvc;

namespace StudyProgressChecker.Controllers
{
    public class ViewSingleStudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
