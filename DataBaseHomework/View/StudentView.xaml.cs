using DataBaseHomework.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DataBaseHomework.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StudentView : Page
    {
        string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "mydb.sqlite");    //建立数据库  
        SQLiteConnection conn;
        public StudentView()
        {
            this.InitializeComponent();
            //建立数据库连接   
            conn = new SQLiteConnection(path);
            //建表         
            Initialize();
        }

        private void Initialize()
        {
            SnoTB.Foreground = new SolidColorBrush(Color.FromArgb(255, 81, 196, 211));
            var datalist = conn.Query<Student>("select *from Student where Sno = ?", Login.Current.UserName.Text);
            var _uername = conn.ExecuteScalar<string>("select Sname from Student where Sno = ?", Login.Current.UserName.Text);
            HelloTB.Text = _uername + "，你好。";
            foreach(var item in datalist)
            {
                SnoTB.Text = "学号：" + item.Sno;
                SexTB.Text = "性别：" + item.Sex;
                AgeTB.Text = "年龄：" + item.Age;
            }
        }
    }
}
