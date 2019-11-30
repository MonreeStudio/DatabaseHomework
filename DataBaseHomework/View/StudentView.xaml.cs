using DataBaseHomework.Models;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
            conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            //建表              
            conn.CreateTable(typeof(Teacher));
            conn.CreateTable(typeof(Student));
            conn.CreateTable(typeof(Course));
        }
    }
}
