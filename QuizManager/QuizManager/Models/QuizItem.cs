using System.Collections.Generic;

namespace QuizManager.Models
{
    public class QuizItem
    {
        public int Id { get; set; }
        public List<string> DescriptionStrings => new List<string>();

        private Dictionary<string, List<string>> LinkedTo = new Dictionary<string, List<string>>();

    }
}