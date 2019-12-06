using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.Models
{
    class Course
    {
        [PrimaryKey]
        public string Cno { get; set; }
        public string Cname { get; set; }
        public double? Credit { get; set; }
        [ForeignKey(typeof(Teacher))]
        public string Tno { get; set; }

        [ManyToOne]
        public Teacher Teacher { get; set; }
    }
}
