using Microsoft.AspNetCore.Mvc;
using StudyProgressChecker.Models;
using System.Collections.Generic;

namespace StudyProgressChecker.Controllers
{
    public class SubjectController : Controller
    {
        public IActionResult Index()
        {
            var allSubjects = Subject.GetAllSubjects();
            return View(allSubjects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (Subject.AddSubject(subject))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Name", "A subject with this name already exists.");
            }
            return View(subject);
        }

        public IActionResult EditAll()
        {
            var allSubjects = Subject.GetAllSubjects();
            return View(allSubjects);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAll(List<Subject> subjects)
        {
            var logMessages = new List<string>();
            logMessages.Add($"EditAll action called with {subjects.Count} subjects");

            if (ModelState.IsValid)
            {
                foreach (var updatedSubject in subjects)
                {
                    var existingSubject = Subject.GetSubject(updatedSubject.Name);
                    if (existingSubject != null)
                    {
                        if (Subject.UpdateSubject(existingSubject.Name, updatedSubject))
                        {
                            logMessages.Add($"Updated Subject - Name: {updatedSubject.Name}, Weight: {updatedSubject.Weight}");
                        }
                        else
                        {
                            logMessages.Add($"Failed to update Subject - Name: {updatedSubject.Name}");
                        }
                    }
                    else
                    {
                        logMessages.Add($"Subject not found - Name: {updatedSubject.Name}");
                    }
                }

                TempData["LogMessages"] = logMessages;
                return RedirectToAction(nameof(Index));
            }

            ViewBag.LogMessages = logMessages;
            return View(subjects);
        }

        public IActionResult Delete(string name)
        {
            var subject = Subject.GetSubject(name);
            if (subject == null)
            {
                return NotFound();
            }
            return View("~/Views/Subject/Delete.cshtml", subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string name)
        {
            Subject.DeleteSubject(name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string name)
        {
            var subject = Subject.GetSubject(name);
            if (subject == null)
            {
                return NotFound();
            }
            ViewBag.OriginalName = name;
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string originalName, Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (Subject.UpdateSubject(originalName, subject))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Name", "A subject with this name already exists or the subject was not found.");
            }
            ViewBag.OriginalName = originalName;
            return View(subject);
        }
    }
}