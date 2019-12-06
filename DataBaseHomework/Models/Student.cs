using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.Models
{
    class Student
    {
        [PrimaryKey]
        public string Sno { get; set; }
        public string Sname { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
    }
}
