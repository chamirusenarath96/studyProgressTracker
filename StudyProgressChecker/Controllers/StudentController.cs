using Microsoft.AspNetCore.Mvc;
using StudyProgressChecker.Models;
using System.Linq;

namespace StudyProgressChecker.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            var students = Student.GetAllStudents();
            return View(students);
        }

        public IActionResult Create()
        {
            ViewBag.UniversityId = Student.GenerateUniversityId();
            ViewBag.Subjects = Subject.GetAllSubjects();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student, List<string> selectedSubjects)
        {
            if (ModelState.IsValid)
            {
                if (Student.AddStudent(student))
                {
                    foreach (var subjectName in selectedSubjects)
                    {
                        Student.AssignSubject(student.UniversityId, subjectName);
                    }
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to add student. University ID may already exist.");
            }
            ViewBag.UniversityId = student.UniversityId;
            ViewBag.Subjects = Subject.GetAllSubjects();
            return View(student);
        }

        public IActionResult Delete(string id)
        {
            var student = Student.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            Student.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AssignSubject(string id)
        {
            var student = Student.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.AllSubjects = Subject.GetAllSubjects();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignSubject(string id, List<string> selectedSubjects)
        {
            var student = Student.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            // Clear existing preferred subjects
            student.PreferredSubjects.Clear();

            // Add selected subjects
            foreach (var subjectName in selectedSubjects)
            {
                Student.AssignSubject(id, subjectName);
            }

            // Remove subjects that were not selected
            var allSubjects = Subject.GetAllSubjects();
            foreach (var subject in allSubjects)
            {
                if (!selectedSubjects.Contains(subject.Name))
                {
                    Student.RemoveSubject(id, subject.Name);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveSubject(string id, string subjectName)
        {
            Student.RemoveSubject(id, subjectName);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult StudentProgress(string id)
        {
            var student = Student.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            var studySessions = StudySession.GetStudySessionsByStudent(id);
            return View(studySessions);
        }
    }
}