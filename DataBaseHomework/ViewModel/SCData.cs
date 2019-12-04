using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.ViewModel
{
    public class SCData
    {
        public string Sno { get; set; }
        public string Cno { get; set; }
        public string Score { get; set; }
    }

    public class SCDataViewModel
    {
        public ObservableCollection<SCData> StudentDatas = new ObservableCollection<SCData>();
        public SCDataViewModel()
        {

        }
    }
}
