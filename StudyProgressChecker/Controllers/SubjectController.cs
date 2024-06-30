using Microsoft.AspNetCore.Mvc;
using StudyProgressChecker.Models;

namespace StudyProgressChecker.Controllers
{
    public class SubjectController : Controller
    {
        // GET: Subjects
        public ActionResult Index()
        {

            // Retrieving all subjects
            var allSubjects = Subject.GetAllSubjects();

            return View(allSubjects);
        }

        // GET: Subjects/Details/5
        public ActionResult Details(string id)
        {
            var subject = Subject.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // Example of handling form submission with validation
        [HttpPost]
        public ActionResult Create(Subject model)
        {
            if (ModelState.IsValid)
            {
                // Validation passed, add the subject
                Subject.AddSubject(model);
                return RedirectToAction("Index");
            }

            // Validation failed, return the view with errors
            return View(model);
        }
    }
}
