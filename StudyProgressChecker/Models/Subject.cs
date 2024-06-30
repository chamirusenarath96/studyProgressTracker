using System.ComponentModel.DataAnnotations;

namespace StudyProgressChecker.Models
{
    public class Subject
    {
        // Properties
        public string Id { get; private set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Subject name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100")]
        public int Weight { get; set; }

        // Constructor
        public Subject()
        {
            Id = Guid.NewGuid().ToString(); // Generates a new GUID as ID
        }

        // Static list to store all created subjects
        private static List<Subject> Subjects = new List<Subject>();

        // Method to add a new subject to the list
        public static void AddSubject(Subject subject)
        {
            Subjects.Add(subject);
        }

        // Method to retrieve all subjects
        public static List<Subject> GetAllSubjects()
        {
            return Subjects;
        }

        // Method to find a subject by its ID
        public static Subject GetSubjectById(string id)
        {
            return Subjects.Find(s => s.Id == id);
        }

        // Method to update subject name
        public void UpdateName(string newName)
        {
            Name = newName;
        }

        // Method to update subject weight
        public void UpdateWeight(int newWeight)
        {
            Weight = newWeight;
        }
    }
}
