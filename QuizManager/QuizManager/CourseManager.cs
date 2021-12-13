using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QuizManager;

public class CourseManager : ListManager<Course>
{
    public ObservableCollection<Course> CoursesList { get; set; }
}