using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.Models
{
    class SC
    {
        [ForeignKey(typeof(Student))]
        public string Sno { get; set; }
        [ForeignKey(typeof(Course))]
        public string Cno { get; set; }
        public double Score { get; set; }
    }
}
