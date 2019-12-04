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
        public CourseDataViewModel CouViewModel = new CourseDataViewModel();
        private string _sex;
        private static StudentData StudentItem;
        private static TeacherData TeacherItem;
        private static CourseData CourseItem;
        public ManagementView()
        {
            this.InitializeComponent();
            conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            AddStudent.Background = new SolidColorBrush(Color.FromArgb(255, 81, 196, 211));
            if (StuList.IsChecked == false && TeaList.IsChecked == false&&CouList.IsChecked==false)
                StuList.IsChecked = true;
        }

        private void RefreshStuList()
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

        private void RefreshTeaList()
        {
            TeaViewModel.TeacherDatas.Clear();
            List<Teacher> datalist = conn.Query<Teacher>("select * from Teacher");
            foreach (var item in datalist)
            {
                try
                {
                    TeaViewModel.TeacherDatas.Add(new TeacherData() { Tname = "姓名：" + item.Tname, Tno = "工号：" + item.Tno, JobTitle = "职称：" + item.JobTitle, Salary = "工资：" + item.Salary.ToString() });
                }
                catch (ArgumentNullException ex)
                {
                    throw ex;
                }
            }
        }
        private void RefreshCouList()
        {
            CouViewModel.CourseDatas.Clear();
            List<Course> datalist = conn.Query<Course>("select * from Course");
            foreach (var item in datalist)
            {
                try
                {
                    CouViewModel.CourseDatas.Add(new CourseData() { Cname = "课程名：" + item.Cname, Cno = "课程号：" + item.Cno, Tno = "任课教师工号："+item.Tno, Credit = "学分：" + item.Credit });
                }
                catch (ArgumentNullException ex)
                {
                    throw ex;
                }
            }
        }
        private async void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            await AddDialog.ShowAsync();
        }


        private async void QueryStudent_Click(object sender, RoutedEventArgs e)
        {
            await QueryDialog.ShowAsync();
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
                    RefreshStuList();
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

        private void QueryAllStudent_Click(object sender, RoutedEventArgs e)
        {
            StuList.IsChecked = true;
            RefreshStuList();
        }

        private void ClearStudentList_Click(object sender, RoutedEventArgs e)
        {
            StuViewModel.StudentDatas.Clear();
        }

        private async void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            await AddDialog2.ShowAsync();
        }

        private async void QueryTeacher_Click(object sender, RoutedEventArgs e)
        {
            await QueryDialog2.ShowAsync();
        }

        private void ClearTeacherList_Click(object sender, RoutedEventArgs e)
        {
            TeaViewModel.TeacherDatas.Clear();
        }

        private void QueryAllTeacher_Click_1(object sender, RoutedEventArgs e)
        {
            TeaList.IsChecked = true;
            RefreshTeaList();
        }

        private void StuList_Checked(object sender, RoutedEventArgs e)
        {
            StudentBorder.Visibility = Visibility.Visible;
            TeacherBorder.Visibility = Visibility.Collapsed;
            CourseBorder.Visibility = Visibility.Collapsed;
        }

        private void TeaList_Checked(object sender, RoutedEventArgs e)
        {
            StudentBorder.Visibility = Visibility.Collapsed;
            TeacherBorder.Visibility = Visibility.Visible;
            CourseBorder.Visibility = Visibility.Collapsed;
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
                conn.Execute("update Student set Sname = ? where Sno = ?", SnameTB2.Text, SnoTB2.Text.Substring(3));
                conn.Execute("update Student set Sex = ? where Sno = ?", SexTB2.SelectedItem.ToString(), SnoTB2.Text.Substring(3));
                conn.Execute("update Student set Age = ? where Sno = ?", AgeTB2.Text, SnoTB2.Text.Substring(3));
                conn.Execute("update Student set Password = ? where Sno = ?", Spassword2.Text, SnoTB2.Text.Substring(3));
                PopupNotice popupNotice = new PopupNotice("修改成功");
                popupNotice.ShowAPopup();
                RefreshStuList();
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void Clear2Btn_Click(object sender, RoutedEventArgs e)
        {
            SnameTB2.Text = "";
            SexTB2.Text = "";
            AgeTB2.Text = "";
            Spassword2.Text = "";
        }

        private void TClearBtn_Click(object sender, RoutedEventArgs e)
        {
            TnameTB.Text = "";
            TnoTB.Text = "";
            JobTitleTB.Text = "";
            SalaryTB.Text = "";
        }

        private async void AddDialog2_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                if (TnoTB.Text != "" && TnameTB.Text != "" && JobTitleTB.Text != "" && SalaryTB.Text != "")
                {
                    conn.Insert(new Teacher() { Tname = TnameTB.Text, Tno = TnoTB.Text, JobTitle = JobTitleTB.Text, Salary = Convert.ToDouble(SalaryTB.Text) });
                    PopupNotice popupNotice = new PopupNotice("添加成功");
                    popupNotice.ShowAPopup();
                    RefreshTeaList();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            catch(FormatException)
            {
                MessageDialog AboutDialog = new MessageDialog("信息格式出现错误！", "提示");
                await AboutDialog.ShowAsync();
            }
            catch
            {
                MessageDialog AboutDialog = new MessageDialog("该教师已存在！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private async void Tupdate_Click(object sender, RoutedEventArgs e)
        {
            TnoTB2.Text = TeacherItem.Tno;
            TnameTB2.Text = TeacherItem.Tname.Substring(3);
            JobTitleTB2.Text = TeacherItem.JobTitle.Substring(3);
            SalaryTB2.Text = TeacherItem.Salary.Substring(3);
            await UpdateDialog2.ShowAsync();
        }

        private async void Tdelete_Click(object sender, RoutedEventArgs e)
        {
            await DeleteDialog2.ShowAsync();
        }

        private void TListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var _item = (e.OriginalSource as FrameworkElement)?.DataContext as TeacherData;
            TeacherItem = _item;
        }

        private void CloseBtn2_Click_1(object sender, RoutedEventArgs e)
        {
            QueryDialog.Hide();
        }

        private void DeleteBtn2_Click(object sender, RoutedEventArgs e)
        {
            conn.Execute("delete from Teacher where Tno = ?", TeacherItem.Tno.Substring(3));
            TeaViewModel.TeacherDatas.Remove(TeacherItem);
            DeleteDialog2.Hide();
            PopupNotice popupNotice = new PopupNotice("删除成功");
            popupNotice.ShowAPopup();
        }

        private void TCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteDialog2.Hide();
        }

        private void TCloseBtn2_Click(object sender, RoutedEventArgs e)
        {
            QueryDialog2.Hide();
        }

        private async void QueryBtn2_Click(object sender, RoutedEventArgs e)
        {
            if (QueryTno.Text != "")
            {
                var datalist = conn.Query<Teacher>("select *from Teacher where Tno = ?", QueryTno.Text);
                if (datalist.Count != 0)
                {
                    TeaViewModel.TeacherDatas.Clear();
                    foreach (var item in datalist)
                    {
                        TeaViewModel.TeacherDatas.Add(new TeacherData() { Tname = "姓名：" + item.Tname, Tno = "工号:" + item.Tno, JobTitle = "职称：" + item.JobTitle, Salary = "年龄：" + item.Salary });
                    }
                    QueryDialog2.Hide();
                    PopupNotice popupNotice = new PopupNotice("查找成功");
                    popupNotice.ShowAPopup();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("该教师不存在！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请输入待查询教师的工号！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void TClear2Btn_Click(object sender, RoutedEventArgs e)
        {
            TnameTB2.Text = "";
            JobTitleTB2.Text = "";
            SalaryTB2.Text = "";
        }

        private async void UpdateDialog2_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (TnameTB2.Text != "" && TnoTB2.Text != "" && JobTitleTB2.Text != "" && SalaryTB2.Text != "")
            {
                conn.Execute("update Teacher set Tname = ? where Tno = ?", TnameTB2.Text, TnoTB2.Text.Substring(3));
                conn.Execute("update Teacher set JobTitle = ? where Tno = ?", JobTitleTB2.Text, TnoTB2.Text.Substring(3));
                conn.Execute("update Teacher set Salary = ? where Tno = ?", SalaryTB2.Text, TnoTB2.Text.Substring(3));
                PopupNotice popupNotice = new PopupNotice("修改成功");
                popupNotice.ShowAPopup();
                RefreshTeaList();
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private async void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            await AddDialog3.ShowAsync();
        }

        private async void QueryCourse_Click(object sender, RoutedEventArgs e)
        {
            await QueryDialog3.ShowAsync();
        }

        private void QueryAllCourse_Click(object sender, RoutedEventArgs e)
        {
            CouList.IsChecked = true;
            RefreshCouList();
        }

        private void ClearCourseList_Click(object sender, RoutedEventArgs e)
        {
            CouViewModel.CourseDatas.Clear();
        }

        private async void Cupdate_Click(object sender, RoutedEventArgs e)
        {
            CnameTB2.Text = CourseItem.Cname.Substring(4);
            CnoTB2.Text = CourseItem.Cno;
            CreditTB2.Text = CourseItem.Credit.Substring(3);
            CTnoTB2.Text = CourseItem.Tno.Substring(7);
            await UpdateDialog3.ShowAsync();
        }

        private async void Cdelete_Click(object sender, RoutedEventArgs e)
        {
            await DeleteDialog3.ShowAsync();
        }

        private void CListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var _item = (e.OriginalSource as FrameworkElement)?.DataContext as CourseData;
            CourseItem = _item;
        }

        private void CouList_Checked(object sender, RoutedEventArgs e)
        {
            StudentBorder.Visibility = Visibility.Collapsed;
            TeacherBorder.Visibility = Visibility.Collapsed;
            CourseBorder.Visibility = Visibility.Visible;
        }

        private async void AddDialog3_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                if (CnoTB.Text != "" && CnameTB.Text != "" && CreditTB.Text != "")
                {
                    conn.Insert(new Course() { Cname = CnameTB.Text, Cno = CnoTB.Text,Tno = CTnoTB.Text, Credit = Convert.ToDouble(CreditTB.Text)});
                    PopupNotice popupNotice = new PopupNotice("添加成功");
                    popupNotice.ShowAPopup();
                    RefreshCouList();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            catch(SQLiteException)
            {
                MessageDialog AboutDialog = new MessageDialog("违反了约束条件！", "提示");
                await AboutDialog.ShowAsync();
            }
            catch (FormatException)
            {
                MessageDialog AboutDialog = new MessageDialog("信息格式出现错误！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void CCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteDialog3.Hide();
        }

        private void DeleteBtn3_Click(object sender, RoutedEventArgs e)
        {
            conn.Execute("delete from Course where Cno = ?", CourseItem.Cno.Substring(4));
            CouViewModel.CourseDatas.Remove(CourseItem);
            DeleteDialog3.Hide();
            PopupNotice popupNotice = new PopupNotice("删除成功");
            popupNotice.ShowAPopup();
        }

        private async void QueryBtn3_Click(object sender, RoutedEventArgs e)
        {
            if (QueryCno.Text != "")
            {
                var datalist = conn.Query<Course>("select *from Course where Cno = ?", QueryCno.Text);
                if (datalist.Count != 0)
                {
                    CouViewModel.CourseDatas.Clear();
                    foreach (var item in datalist)
                    {
                        CouViewModel.CourseDatas.Add(new CourseData() { Cname = "课程名：" + item.Cname, Cno = "课程号:" + item.Cno,Tno = "任课老师工号："+item.Tno, Credit = "学分：" + item.Credit });
                    }
                    QueryDialog3.Hide();
                    PopupNotice popupNotice = new PopupNotice("查找成功");
                    popupNotice.ShowAPopup();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("该课程不存在！", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请输入待查询课程的课程号！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void CCloseBtn2_Click(object sender, RoutedEventArgs e)
        {
            QueryDialog3.Hide();
        }

        private async void UpdateDialog3_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (CnameTB2.Text != "" && CnoTB2.Text != ""&&CTnoTB2.Text!="" && CreditTB2.Text != "")
            {
                conn.Execute("update Course set Cname = ? where Cno = ?", CnameTB2.Text, CnoTB2.Text.Substring(4));
                conn.Execute("update Course set Credit = ? where Cno = ?", CreditTB2.Text, CnoTB2.Text.Substring(4));
                PopupNotice popupNotice = new PopupNotice("修改成功");
                popupNotice.ShowAPopup();
                RefreshCouList();
            }
            else
            {
                MessageDialog AboutDialog = new MessageDialog("请将信息填写完整！", "提示");
                await AboutDialog.ShowAsync();
            }
        }

        private void CClear2Btn_Click(object sender, RoutedEventArgs e)
        {
            CnameTB2.Text = "";
            CreditTB2.Text = "";
        }

        private void CClearBtn_Click(object sender, RoutedEventArgs e)
        {
            CnameTB.Text = "";
            CnoTB.Text = "";
            CreditTB.Text = "";
        }

        private async void CountBtn_Click(object sender, RoutedEventArgs e)
        {
            //var pList = conn.Query<Teacher>("select AVG(Salary) from Teacher where JobTitle = ?","教授");
            //pCount.Text = "数量：" + pList.Count();
            await CountDialog.ShowAsync();
            
        }
    }
}
