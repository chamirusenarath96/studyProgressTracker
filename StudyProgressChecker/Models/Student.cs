using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StudyProgressChecker.Models
{
    public class Student
    {
        private static Dictionary<string, Student> Students = new Dictionary<string, Student>();
        private static bool defaultStudentsAdded = false;

        static Student()
        {
            AddDefaultStudents();
        }

        private static void AddDefaultStudents()
        {
            if (!defaultStudentsAdded)
            {
                // Ensure default subjects are added
                var allSubjects = Subject.GetAllSubjects();

                // Create and add default students
                var student1 = new Student
                {
                    UniversityId = GenerateUniversityId(),
                    FirstName = "John",
                    LastName = "Doe",
                    YearOfStudy = 2,
                    PreferredSubjects = new List<Subject>
                    {
                        Subject.GetSubject("Mathematics"),
                        Subject.GetSubject("Physics")
                    }
                };
                AddStudent(student1);

                var student2 = new Student
                {
                    UniversityId = GenerateUniversityId(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    YearOfStudy = 3,
                    PreferredSubjects = new List<Subject>
                    {
                        Subject.GetSubject("Chemistry"),
                        Subject.GetSubject("Biology")
                    }
                };
                AddStudent(student2);

                var student3 = new Student
                {
                    UniversityId = GenerateUniversityId(),
                    FirstName = "Alice",
                    LastName = "Johnson",
                    YearOfStudy = 1,
                    PreferredSubjects = new List<Subject>
                    {
                        Subject.GetSubject("History"),
                        Subject.GetSubject("Mathematics")
                    }
                };
                AddStudent(student3);

                defaultStudentsAdded = true;
            }
        }

        [Key]
        public string UniversityId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [Range(1, 4)]
        [Display(Name = "Year of Study")]
        public int YearOfStudy { get; set; }

        public List<Subject> PreferredSubjects { get; set; } = new List<Subject>();

        public static string GenerateUniversityId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static List<Student> GetAllStudents()
        {
            AddDefaultStudents();
            return Students.Values.ToList();
        }

        public static Student GetStudent(string id)
        {
            AddDefaultStudents();
            Students.TryGetValue(id, out Student student);
            return student;
        }

        public static bool AddStudent(Student student)
        {
            if (!Students.ContainsKey(student.UniversityId))
            {
                Students[student.UniversityId] = student;
                return true;
            }
            return false;
        }

        public static bool UpdateStudent(Student student)
        {
            if (Students.ContainsKey(student.UniversityId))
            {
                Students[student.UniversityId] = student;
                return true;
            }
            return false;
        }

        public static bool DeleteStudent(string id)
        {
            return Students.Remove(id);
        }

        public static void AssignSubject(string studentId, string subjectName)
        {
            if (Students.TryGetValue(studentId, out Student student))
            {
                var subject = Subject.GetSubject(subjectName);
                if (subject != null && !student.PreferredSubjects.Any(s => s.Name == subjectName))
                {
                    student.PreferredSubjects.Add(subject);
                }
            }
        }

        public static void RemoveSubject(string studentId, string subjectName)
        {
            if (Students.TryGetValue(studentId, out Student student))
            {
                student.PreferredSubjects.RemoveAll(s => s.Name == subjectName);
            }
        }
    }
}