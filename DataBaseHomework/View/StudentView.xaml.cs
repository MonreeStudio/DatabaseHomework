using DataBaseHomework.Models;
using DataBaseHomework.ViewModel;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
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
        public StudentDataViewModel StuViewModel = new StudentDataViewModel();
        private string _sex;
        public StudentView()
        {
            this.InitializeComponent();
            conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            AddStudent.Background = new SolidColorBrush(Color.FromArgb(255, 81, 196, 211));
        }

        private async void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            await AddDialog.ShowAsync();
        }

        private async void DeletStudent_Click(object sender, RoutedEventArgs e)
        {
            await DeleteDialog.ShowAsync();
        }

        private void UpdateStudent_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void QueryStudent_Click(object sender, RoutedEventArgs e)
        {
            await QueryDialog.ShowAsync();
        }

        private void QueryAll_Click(object sender, RoutedEventArgs e)
        {
            StuViewModel.StudentDatas.Clear();
            List<Student> datalist = conn.Query<Student>("select * from Student");
            foreach(var item in datalist)
            {
                try
                {
                    StuViewModel.StudentDatas.Add(new StudentData() { Sname = "姓名："+item.Sname, Sno = "学号："+item.Sno, Sex = "性别：" + item.Sex, Age = "年龄：" + item.Age });
                }
                catch(ArgumentNullException ex)
                {
                    throw ex;
                }
            }
        }

        private async void AddDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                if (SnameTB.Text != "" && SnoTB.Text != "" && _sex != "" && AgeTB.Text != "")
                {
                    conn.Insert(new Student() { Sname = SnameTB.Text, Sno = SnoTB.Text, Sex = _sex, Age = Convert.ToInt32(AgeTB.Text) });
                    PopupNotice popupNotice = new PopupNotice("添加成功");
                    popupNotice.ShowAPopup();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            catch
            {
                MessageDialog AboutDialog = new MessageDialog("该学生已存在！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void ClearList_Click(object sender, RoutedEventArgs e)
        {
            StuViewModel.StudentDatas.Clear();
        }

        private void SexTB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SexTB.SelectedItem!=null)
                _sex = SexTB.SelectedItem.ToString();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            SnoTB.Text = "";
            SnameTB.Text = "";
            _sex = "";
            SexTB.SelectedItem = null;
            AgeTB.Text = "";
        }


        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteSno.Text != "")
            {
                var datalist = conn.Query<Student>("select *from Student where Sno = ?", DeleteSno.Text);
                if (datalist.Count != 0)
                {
                    conn.Execute("delete from Student where Sno = ?", DeleteSno.Text);
                    
                    DeleteDialog.Hide();
                    PopupNotice popupNotice = new PopupNotice("删除成功");
                    popupNotice.ShowAPopup();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("该学生不存在！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请输入待删除学生的学号！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteDialog.Hide();
        }

        private async void QueryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(QuerySno.Text!="")
            {
                var datalist = conn.Query<Student>("select *from Student where Sno = ?", QuerySno.Text);
                if (datalist.Count != 0)
                {
                    StuViewModel.StudentDatas.Clear();
                    foreach(var item in datalist)
                    {
                        StuViewModel.StudentDatas.Add(new StudentData() { Sname = "姓名：" + item.Sname, Sno = "学号" + item.Sno, Sex = "性别：" + item.Sex, Age = "年龄：" + item.Age.ToString() });
                    }
                    QueryDialog.Hide();
                    PopupNotice popupNotice = new PopupNotice("查找成功");
                    popupNotice.ShowAPopup();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("该学生不存在！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请输入待查询学生的学号！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void CloseBtn2_Click(object sender, RoutedEventArgs e)
        {
            QueryDialog.Hide();
        }
    }
}
