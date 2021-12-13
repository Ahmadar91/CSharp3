using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace QuizManager.ViewModel;

public class View
{
    private readonly MainWindow _mainWindow;
    public CourseManager CourseManager { get; set; }
    public ObservableCollection<CoursesViewModel> Courses { get; set; }

    public View(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        CourseManager = new CourseManager();
        _mainWindow.CoursesList.DataContext = CourseManager.CoursesList;
        _mainWindow.addCourseBtn.Click += addCourseBtn_Click;
        _mainWindow.CoursesList.SelectionChanged += CoursesList_SelectionChanged
    }



    private void addCourseBtn_Click(object sender, RoutedEventArgs e)
    {
        var course = new Course()
        {
            Id = _mainWindow.IdText.Text,
            Name = _mainWindow.NameText.Text,
            Modules = AggregateString(_mainWindow.ModulesText.Text)
        };
        CourseManager.Add(course);
        Courses = new ObservableCollection<CoursesViewModel>(Map(CourseManager.GetAll()));
        _mainWindow.CoursesList.ItemsSource = Courses;
        _mainWindow.CoursesList.ItemsSource = Courses;
    }
    private void CoursesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        //_mainWindow.ModuleList.ItemsSource;
    }

public List<string> AggregateString(string value)
    {
        var result = new List<string>();

        if (string.IsNullOrEmpty(value))
        {
            return result;
        }
        var stringItems = value.Split(",").ToList();

        foreach (var item in stringItems)
        {
            result.Add(item);
        }
        return result;
    }

    public List<CoursesViewModel> Map(List<Course> courses)
    {
        return courses.Select(x => new CoursesViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Modules = x.Modules
        }).ToList();
    }
}


public class CoursesViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Modules { get; set; }
}