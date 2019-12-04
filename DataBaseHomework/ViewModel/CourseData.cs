using DataBaseHomework.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.ViewModel
{
    public class CourseData
    {
        public string Cno { get; set; }
        public string Cname { get; set; }
        public string Credit { get; set; }
        public string Tno { get; set; }
    }

    public class CourseDataViewModel
    {
        public ObservableCollection<CourseData> CourseDatas = new ObservableCollection<CourseData>();
        public CourseDataViewModel()
        {

        }
    }
}
