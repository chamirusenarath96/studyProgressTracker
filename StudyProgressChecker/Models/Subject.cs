using System;
using System.Collections.Generic;

namespace StudyProgressChecker.Models
{
    public class Subject
    {
        private static Dictionary<string, Subject> Subjects = new Dictionary<string, Subject>(StringComparer.OrdinalIgnoreCase);
        private static bool defaultSubjectsAdded = false;

        public string Name { get; set; }
        public int Weight { get; set; }

        public static List<Subject> GetAllSubjects()
        {
            if (!defaultSubjectsAdded)
            {
                AddDefaultSubjects();
                defaultSubjectsAdded = true;
            }
            return new List<Subject>(Subjects.Values);
        }

        private static void AddDefaultSubjects()
        {
            if (Subjects.Count == 0)
            {
                AddSubject(new Subject { Name = "Mathematics", Weight = 10 });
                AddSubject(new Subject { Name = "Physics", Weight = 8 });
                AddSubject(new Subject { Name = "Chemistry", Weight = 8 });
                AddSubject(new Subject { Name = "Biology", Weight = 7 });
                AddSubject(new Subject { Name = "History", Weight = 6 });
            }
        }

        public static bool AddSubject(Subject subject)
        {
            if (!Subjects.ContainsKey(subject.Name))
            {
                Subjects[subject.Name] = subject;
                return true;
            }
            return false;
        }

        public static bool UpdateSubject(string oldName, Subject updatedSubject)
        {
            if (Subjects.ContainsKey(oldName))
            {
                if (oldName != updatedSubject.Name && Subjects.ContainsKey(updatedSubject.Name))
                {
                    return false; // New name already exists
                }
                Subjects.Remove(oldName);
                Subjects[updatedSubject.Name] = updatedSubject;
                return true;
            }
            return false;
        }

        public static bool DeleteSubject(string name)
        {
            return Subjects.Remove(name);
        }

        public static bool SubjectExists(string name)
        {
            return Subjects.ContainsKey(name);
        }

        public static Subject GetSubject(string name)
        {
            Subjects.TryGetValue(name, out Subject subject);
            return subject;
        }
    }
}