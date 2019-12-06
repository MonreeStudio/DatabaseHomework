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
    public sealed partial class Login : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public static Login Current;
        public Login()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            Current = this;
            LoginGrid.Background = ColorfulBrush(Color.FromArgb(255,81,196,211), 0.8);
            LoginBtn.Foreground = new SolidColorBrush(Color.FromArgb(255, 81, 196, 211));
        }

        public AcrylicBrush ColorfulBrush(Color temp, double tintOpacity)
        {
            AcrylicBrush myBrush = new AcrylicBrush();
            myBrush.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
            myBrush.TintColor = temp;
            myBrush.TintColor = temp;
            myBrush.FallbackColor = temp;
            myBrush.TintOpacity = tintOpacity;
            return myBrush;
        }

        private void StuBtn_Checked(object sender, RoutedEventArgs e)
        {
            UserName.PlaceholderText = "学号";
        }

        private void ManageBtn_Checked(object sender, RoutedEventArgs e)
        {
            UserName.PlaceholderText = "管理员账号";
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if(StuBtn.IsChecked==true)
            {
                localSettings.Values["Sno"] = "1706300005";
                localSettings.Values["StuPassword"] = "08561X";
                if (localSettings.Values["Sno"].ToString().Equals(UserName.Text) && localSettings.Values["StuPassword"].Equals(UserPassword.Password))
                {
                    MainPage.Current.MyFrame.Navigate(typeof(StudentView));
                    PopupNotice popupNotice = new PopupNotice("登录成功");
                    popupNotice.ShowAPopup();
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("学号或密码错误！请重新输入。", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
            else
            {
                if(ManageBtn.IsChecked==true)
                {
                    localSettings.Values["Mno"] = "1706300005";
                    localSettings.Values["ManagePassword"] = "19081908";
                    if (localSettings.Values["Mno"].ToString().Equals(UserName.Text) && localSettings.Values["ManagePassword"].Equals(UserPassword.Password))
                    {
                        MainPage.Current.MyFrame.Navigate(typeof(ManagementView));
                        PopupNotice popupNotice = new PopupNotice("登录成功");
                        popupNotice.ShowAPopup();
                    }
                    else
                    {
                        MessageDialog AboutDialog = new MessageDialog("管理员账号或密码错误！请重新输入。", "提示");
                        await AboutDialog.ShowAsync();
                    }
                }
                else
                {
                    MessageDialog AboutDialog = new MessageDialog("请选择用户类型。", "提示");
                    await AboutDialog.ShowAsync();
                }
            }
        }
    }
}
