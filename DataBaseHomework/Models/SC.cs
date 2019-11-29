using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHomework.Models
{
    class SC
    {
        [PrimaryKey]
        public string Sno { get; set; }
        public string Tno { get; set; }
        public string Cno { get; set; }
        public double Grade { get; set; }
    }
}
