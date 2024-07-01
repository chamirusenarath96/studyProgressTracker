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

        // Display edit subject form
        public IActionResult Edit(string id)
        {
            var subject = Subject.GetSubjectById(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // Handle edit subject form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subject subject)
        {
            if (ModelState.IsValid)
            {
                var existingSubject = Subject.GetSubjectById(subject.Id);
                if (existingSubject == null)
                {
                    return NotFound();
                }
                existingSubject.UpdateName(subject.Name);
                existingSubject.UpdateWeight(subject.Weight);
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }
    }
}
