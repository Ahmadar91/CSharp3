using System.Collections.Generic;

namespace QuizManager.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Modules { get; set; }

    }
}