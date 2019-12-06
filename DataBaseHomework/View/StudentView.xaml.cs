using DataBaseHomework.Models;
using DataBaseHomework.ViewModel;
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
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "mydb.sqlite");    //建立数据库  
        SQLiteConnection conn;
        public StudentDataViewModel StuViewModel = new StudentDataViewModel();
        public TeacherDataViewModel TeaViewModel = new TeacherDataViewModel();
        public CourseDataViewModel CouViewModel = new CourseDataViewModel();
        public SCDataViewModel SCViewModel = new SCDataViewModel();
        private static CourseData SelectedItem;
        private static CourseData MyCourseSelectedItem;
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
            try
            {
                var datalist = conn.Query<Student>("select *from Student where Sno = ?", Login.Current.UserName.Text);
                var _username = conn.ExecuteScalar<string>("select Sname from Student where Sno = ?", Login.Current.UserName.Text);
                HelloTB.Text = _username + "，你好。";
                foreach (var item in datalist)
                {
                    SnoTB.Text = "学号：" + item.Sno;
                    SexTB.Text = "性别：" + item.Sex;
                    AgeTB.Text = "年龄：" + item.Age;
                    RefreshTotalCredit();
                }
            }
            catch
            {

            }
        }

        private void RefreshTotalCredit()
        {
            double totalCredit = 0;
            var datalist = conn.Query<Course>("select *from Course");
            foreach(var item in datalist)
            {
                var isChooseFlag = Login.Current.UserName.Text + item.Cno;
                if (localSettings.Values[isChooseFlag] != null && localSettings.Values[isChooseFlag].Equals(true))
                    totalCredit += Convert.ToDouble(item.Credit);
            }
            TotalCreditTB.Text = "总学分：" + totalCredit;
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickItem = (CourseData)e.ClickedItem;
            SelectedItem = clickItem;
            var _Tname = conn.ExecuteScalar<string>("select Tname from Teacher where Tno = (select Tno from Course where Tno = ?)", SelectedItem.Tno.Substring(5));
            TnameTB.Text = "教师名：" +_Tname;
            CnoTB.Text = SelectedItem.Cno;
            CnameTB.Text = SelectedItem.Cname;
            CreditTB.Text = SelectedItem.Credit;
            var isChoose = Login.Current.UserName.Text + CnoTB.Text.Substring(4);
            if (localSettings.Values[isChoose] == null)
            {
                localSettings.Values[isChoose] = false;
                ChooseToggle.IsChecked = false;
                ChooseToggle.Content = "选课";

            }
            else
            {
                var _userName = Login.Current.UserName.Text;
                if (localSettings.Values[isChoose].Equals(true))
                {
                    conn.Execute("delete from SC where Sno = ? and Cno = ?", _userName, SelectedItem.Cno);
                    ChooseToggle.IsChecked = true;
                    ChooseToggle.Content = "退选";
                    
                }
                else
                {
                    conn.Insert(new SC() { Sno = _userName, Cno = SelectedItem.Cno });
                    ChooseToggle.IsChecked = false;
                    ChooseToggle.Content = "选课";
                    
                }
            }
            await ChooseCourseDialog.ShowAsync();
        }

        private void ShowCourseListBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseBorder.Visibility = Visibility.Visible;
            MyCourseBorder.Visibility = Visibility.Collapsed;
            CouViewModel.CourseDatas.Clear();
            var datalist = conn.Query<Course>("select *from Course");
            foreach(var item in datalist)
            {
                CouViewModel.CourseDatas.Add(new CourseData() { Cname = "课程名：" + item.Cname, Cno = "课程号：" + item.Cno,Tno = "教师工号：" + item.Tno, Credit = "学分：" + item.Credit });
            }
        }

        private void ChooseToggle_Click(object sender, RoutedEventArgs e)
        {
            var isChoose = Login.Current.UserName.Text + CnoTB.Text.Substring(4);
            if (ChooseToggle.Content.ToString()=="选课")
            {
                ChooseToggle.Content = "退选";
                localSettings.Values[isChoose] = true;
                RefreshTotalCredit();
            }
            else
            {
                ChooseToggle.Content = "选课";
                localSettings.Values[isChoose] = false;
                RefreshTotalCredit();
            }
        }

        private async void UpdateInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            var datalist = conn.Query<Student>("select *from Student where Sno = ?", Login.Current.UserName.Text);
            var _username = conn.ExecuteScalar<string>("select Sname from Student where Sno = ?", Login.Current.UserName.Text);
            foreach (var item in datalist)
            {
                SNameTB2.Text = "姓名：" + _username;
                SnoTB2.Text = "学号：" + item.Sno;
                SexTB2.Text = item.Sex;
                AgeTB2.Text = item.Age.ToString();
            }
            await UpdateDialog.ShowAsync();
        }

        private async void UpdatePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            await UpdatePasswordDialog.ShowAsync();
        }

        private void ShowMyCourseListBtn_Click(object sender, RoutedEventArgs e)
        {
            CourseBorder.Visibility = Visibility.Collapsed;
            MyCourseBorder.Visibility = Visibility.Visible;
            CouViewModel.CourseDatas.Clear();
            var datalist = conn.Query<Course>("select *from Course");
            foreach (var item in datalist)
            {
                var isChooseFlag = Login.Current.UserName.Text + item.Cno;
                if (localSettings.Values[isChooseFlag] != null && localSettings.Values[isChooseFlag].Equals(true))
                    CouViewModel.CourseDatas.Add(new CourseData() { Cname = "课程名：" + item.Cname, Cno = "课程号：" + item.Cno, Tno = "教师工号：" + item.Tno, Credit = "学分：" + item.Credit });
            }
        }

        private async void MyCourseList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickItem = (CourseData)e.ClickedItem;
            MyCourseSelectedItem = clickItem;
            var _Tname = conn.ExecuteScalar<string>("select Tname from Teacher where Tno = (select Tno from Course where Tno = ?)", MyCourseSelectedItem.Tno.Substring(5));
            myTnameTB.Text = "任课老师：" + _Tname;
            myCnameTB.Text = MyCourseSelectedItem.Cname;
            myCnoTB.Text = MyCourseSelectedItem.Cno;
            try
            {
                var _score = conn.ExecuteScalar<double>("select Score from SC where Sno = ? and Cno = ?", Login.Current.UserName.Text, MyCourseSelectedItem.Cno.Substring(4));
                myScoreTB.Text = "成绩：" + _score;
            }
            catch
            {

            }
            await MyCourseDialog.ShowAsync();
        }

        private async void UpdateDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (SNameTB2.Text != "" && SnoTB2.Text != "" && SexTB2.Text != "" && AgeTB2.Text != "")
            {
                conn.Execute("update Student set Sex = ? where Sno = ?", SexTB2.Text, SnoTB2.Text.Substring(3));
                conn.Execute("update Student set Age = ? where Sno = ?", AgeTB2.Text, SnoTB2.Text.Substring(3));
                PopupNotice popupNotice = new PopupNotice("修改成功");
                popupNotice.ShowAPopup();
                Initialize();
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private async void UpdatePasswordDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if(OldPasswordTB.Text!=""&&NewPasswordTB.Text!=""&&ConfirmNewPasswordTB.Text!="")
            {
                var _password = conn.ExecuteScalar<string>("select Password from Student where Sno = ?", Login.Current.UserName.Text);
                if(OldPasswordTB.Text==_password)
                {
                    if (NewPasswordTB.Text == ConfirmNewPasswordTB.Text)
                    {
                        conn.Execute("update Student set Password = ? where Sno = ?", NewPasswordTB.Text, Login.Current.UserName.Text);
                        PopupNotice popupNotice = new PopupNotice("修改成功");
                        popupNotice.ShowAPopup();
                    }
                    else
                    {
                        MessageDialog AboutDialog = new MessageDialog("新密码两次输入不一致！", "提示");
                        await AboutDialog.ShowAsync();
                    }
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("原密码错误！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                await AboutDialog.ShowAsync();
            }
        }
    }
}
