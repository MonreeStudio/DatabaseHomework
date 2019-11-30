using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.ViewModel
{
    public class TeacherData
    {
        public string Tno { get; set; }
        public string Tname { get; set; }
        public string JobTitle { get; set; }
        public string Salary { get; set; }
    }

    public class TeacherDataViewModel
    {
        public ObservableCollection<TeacherData> TeacherDatas = new ObservableCollection<TeacherData>();
        public TeacherDataViewModel()
        {

        }
    }
}
