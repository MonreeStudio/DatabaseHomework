using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.Models
{
    class Teacher
    {
        [PrimaryKey]
        public string Tno { get; set; }
        public string Tname { get; set; }
        public string JobTitle { get; set; }
        public double Salary { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Course> TeachCourses { get; set; }
    }
}
