using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StudyProgressChecker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyProgressChecker.Controllers
{
    public class StudySessionController : Controller
    {
        public IActionResult Index()
        {
            var studySessions = StudySession.GetAllStudySessions();
            return View(studySessions);
        }

        public IActionResult Create()
        {
            var students = Student.GetAllStudents();
            ViewBag.Students = new SelectList(students, "UniversityId", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudySession studySession)
        {
            if (ModelState.IsValid)
            {
                studySession.DateAndStartTime = studySession.DateAndStartTime.Date;
                StudySession.AddStudySession(studySession);
                return RedirectToAction(nameof(Index));
            }
            var students = Student.GetAllStudents();
            ViewBag.Students = new SelectList(students, "UniversityId", "FullName");
            return View(studySession);
        }

        [HttpGet]
        public JsonResult GetStudentSubjects(string studentId)
        {
            var student = Student.GetStudent(studentId);
            if (student != null)
            {
                return Json(student.PreferredSubjects);
            }
            return Json(new { });
        }

        public IActionResult StudentProgress(string id)
        {
            var student = Student.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            var studySessions = StudySession.GetStudySessionsByStudent(id);
            ViewBag.StudentName = $"{student.FirstName} {student.LastName}";
            return View(studySessions);
        }
    }
}