using System.Collections.Generic;
using System.Linq;

namespace QuizManager;

public class Course
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Modules { get; set; }

}