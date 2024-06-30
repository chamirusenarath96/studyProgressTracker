using System.ComponentModel.DataAnnotations;

namespace StudyProgressChecker.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public string StudentID { get; set; }
        public DateTime StudyTime { get; set; }
        public DateTime BreakTime { get; set; }
        public Subjects Subject { get; set; }
    }
}
