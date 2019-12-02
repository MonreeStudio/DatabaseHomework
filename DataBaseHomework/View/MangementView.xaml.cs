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
    public sealed partial class ManagementView : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "mydb.sqlite");    //建立数据库  
        SQLiteConnection conn;
        public StudentDataViewModel StuViewModel = new StudentDataViewModel();
        public TeacherDataViewModel TeaViewModel = new TeacherDataViewModel();
        private string _sex;
        private static StudentData StudentItem;
        public ManagementView()
        {
            this.InitializeComponent();
            conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            AddStudent.Background = new SolidColorBrush(Color.FromArgb(255, 81, 196, 211));
            if (StuList.IsChecked == false && TeaList.IsChecked == false)
                StuList.IsChecked = true;
        }

        private async void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            await AddDialog.ShowAsync();
        }


        private async void QueryStudent_Click(object sender, RoutedEventArgs e)
        {
            await QueryDialog.ShowAsync();
        }

        private void QueryAll_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void AddDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                if (SnameTB.Text != "" && SnoTB.Text != "" && _sex != "" && AgeTB.Text != ""&&Spassword.Text!="")
                {
                    conn.Insert(new Student() { Sname = SnameTB.Text, Sno = SnoTB.Text, Sex = _sex, Age = Convert.ToInt32(AgeTB.Text), Password = Spassword.Text });
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


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            conn.Execute("delete from Student where Sno = ?", StudentItem.Sno.Substring(3));
            StuViewModel.StudentDatas.Remove(StudentItem);
            DeleteDialog.Hide();
            PopupNotice popupNotice = new PopupNotice("删除成功");
            popupNotice.ShowAPopup();
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
                        StuViewModel.StudentDatas.Add(new StudentData() { Sname = "姓名：" + item.Sname, Sno = "学号:" + item.Sno, Sex = "性别：" + item.Sex, Age = "年龄：" + item.Age.ToString(), Password = "密码：" + item.Password });
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

        private void QueryAllTeacher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QueryAllStudent_Click(object sender, RoutedEventArgs e)
        {
            StuViewModel.StudentDatas.Clear();
            List<Student> datalist = conn.Query<Student>("select * from Student");
            foreach (var item in datalist)
            {
                try
                {
                    StuViewModel.StudentDatas.Add(new StudentData() { Sname = "姓名：" + item.Sname, Sno = "学号：" + item.Sno, Sex = "性别：" + item.Sex, Age = "年龄：" + item.Age, Password = "密码：" + item.Password });
                }
                catch (ArgumentNullException ex)
                {
                    throw ex;
                }
            }
        }

        private void ClearStudentList_Click(object sender, RoutedEventArgs e)
        {
            StuViewModel.StudentDatas.Clear();
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeletTeacher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateTeacher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QueryTeacher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearTeacherList_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void QueryAllTeacher_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void StuList_Checked(object sender, RoutedEventArgs e)
        {
            StudentBorder.Visibility = Visibility.Visible;
            TeacherBorder.Visibility = Visibility.Collapsed;
        }

        private void TeaList_Checked(object sender, RoutedEventArgs e)
        {
            StudentBorder.Visibility = Visibility.Collapsed;
            TeacherBorder.Visibility = Visibility.Visible;
        }

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var _item = (e.OriginalSource as FrameworkElement)?.DataContext as StudentData;
            StudentItem = _item;
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            await DeleteDialog.ShowAsync();
        }

        private async void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            SnoTB2.Text = StudentItem.Sno;
            SnameTB2.Text = StudentItem.Sname.Substring(3);
            if (StudentItem.Sex.Substring(3).Equals("男"))
                SexTB2.SelectedIndex = 0;
            else
                SexTB2.SelectedIndex = 1;
            AgeTB2.Text = StudentItem.Age.Substring(3);
            Spassword2.Text = StudentItem.Password.Substring(3);
            await UpdateDialog.ShowAsync();    
        }

        private async void UpdateDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if(SnameTB2.Text!=""&&SnoTB2.Text!=""&&SexTB2!=null&&AgeTB2.Text!=""&&Spassword2.Text!="")
            {
                
                conn.Execute("update Student set Sno = ? where Sno = ?", SnoTB2.Text,SnoTB2.Text);
                conn.Execute("update Student set Sname = ? where Sno = ?", SnameTB2.Text, SnoTB2.Text);
                conn.Execute("update Student set Sex = ? where Sno = ?", SexTB2.SelectedItem.ToString(), SnoTB2.Text);
                conn.Execute("update Student set Age = ? where Sno = ?", AgeTB2.Text, SnoTB2.Text);
                conn.Execute("update Student set Password = ? where Sno = ?", Spassword2.Text, SnoTB2.Text);
                //conn.Insert(new Student() { Sname = SnameTB.Text, Sno = SnoTB.Text, Sex = _sex, Age = Convert.ToInt32(AgeTB.Text), Password = Spassword.Text });
                PopupNotice popupNotice = new PopupNotice("修改成功");
                popupNotice.ShowAPopup();
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                await AboutDialog.ShowAsync();
            }

        }

        private void SexTB2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Clear2Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
