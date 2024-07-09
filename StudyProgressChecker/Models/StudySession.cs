using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StudyProgressChecker.Models
{
    public class StudySession : IValidatableObject
    {
        private static Dictionary<int, StudySession> StudySessions = new Dictionary<int, StudySession>();
        private static int nextId = 1;
        private static bool defaultSessionsAdded = false;

        static StudySession()
        {
            AddDefaultStudySessions();
        }

        private static void AddDefaultStudySessions()
        {
            if (!defaultSessionsAdded)
            {
                var students = Student.GetAllStudents();
                var subjects = Subject.GetAllSubjects();

                // Create and add default study sessions
                AddStudySession(new StudySession
                {
                    StudentId = students[0].UniversityId,
                    Subject = subjects[0],
                    DateAndStartTime = DateTime.Now.AddDays(-5),
                    StudiedTimeMinutes = 120,
                    KnowledgeGained = 7
                });

                AddStudySession(new StudySession
                {
                    StudentId = students[1].UniversityId,
                    Subject = subjects[1],
                    DateAndStartTime = DateTime.Now.AddDays(-3),
                    StudiedTimeMinutes = 90,
                    KnowledgeGained = 6
                });

                AddStudySession(new StudySession
                {
                    StudentId = students[2].UniversityId,
                    Subject = subjects[2],
                    DateAndStartTime = DateTime.Now.AddDays(-1),
                    StudiedTimeMinutes = 180,
                    KnowledgeGained = 8
                });

                AddStudySession(new StudySession
                {
                    StudentId = students[0].UniversityId,
                    Subject = subjects[1],
                    DateAndStartTime = DateTime.Now.AddDays(-2),
                    StudiedTimeMinutes = 150,
                    KnowledgeGained = 7
                });

                AddStudySession(new StudySession
                {
                    StudentId = students[1].UniversityId,
                    Subject = subjects[0],
                    DateAndStartTime = DateTime.Now.AddDays(-4),
                    StudiedTimeMinutes = 60,
                    KnowledgeGained = 5
                });

                defaultSessionsAdded = true;
            }
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }

        [Required]
        public Subject Subject { get; set; }

        [Required]
        [Display(Name = "Date and Start Time")]
        public DateTime DateAndStartTime { get; set; }

        [Required]
        [Display(Name = "Studied Time (minutes)")]
        [Range(15, 8 * 60, ErrorMessage = "Studied time must be between 15 minutes and 8 hours.")]
        [FifteenMinuteInterval(ErrorMessage = "Studied time must be in 15-minute intervals.")]
        public int StudiedTimeMinutes { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Knowledge gained must be between 1 and 10.")]
        [Display(Name = "Knowledge Gained (1-10)")]
        public int KnowledgeGained { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StudiedTimeMinutes > 8 * 60)
            {
                yield return new ValidationResult(
                    "The studied time cannot exceed 8 hours.",
                    new[] { nameof(StudiedTimeMinutes) });
            }
        }

        public static List<StudySession> GetAllStudySessions()
        {
            AddDefaultStudySessions();
            return StudySessions.Values.ToList();
        }

        public static StudySession GetStudySession(int id)
        {
            AddDefaultStudySessions();
            StudySessions.TryGetValue(id, out StudySession session);
            return session;
        }

        public static void AddStudySession(StudySession session)
        {
            session.Id = nextId++;
            StudySessions[session.Id] = session;
        }

        public static bool UpdateStudySession(StudySession updatedSession)
        {
            if (StudySessions.ContainsKey(updatedSession.Id))
            {
                StudySessions[updatedSession.Id] = updatedSession;
                return true;
            }
            return false;
        }

        public static bool DeleteStudySession(int id)
        {
            return StudySessions.Remove(id);
        }

        public static List<StudySession> GetStudySessionsByStudent(string studentId)
        {
            AddDefaultStudySessions();
            return StudySessions.Values
                .Where(s => s.StudentId == studentId)
                .OrderBy(s => s.DateAndStartTime)
                .ToList();
        }

        public static List<StudySession> GetStudySessionsBySubject(string subjectName)
        {
            AddDefaultStudySessions();
            return StudySessions.Values.Where(s => s.Subject.Name == subjectName).ToList();
        }
    }

    public class FifteenMinuteIntervalAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int minutes)
            {
                return minutes % 15 == 0;
            }
            return false;
        }
    }
}