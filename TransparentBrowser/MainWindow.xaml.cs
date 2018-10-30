using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TransparentBrowser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.KeyDown += MainWindow_KeyDown;
            this.Closing += MainWindow_Closing;
            this.Opacity = Properties.Settings.Default.opacity;
            web.Address = url.Text = Properties.Settings.Default.url;
            this.ResizeMode = Properties.Settings.Default.canresize ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
            this.Topmost = Properties.Settings.Default.topmost;
            helptxt.Visibility = Properties.Settings.Default.showhelp ? Visibility.Visible : Visibility.Hidden;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.url = url.Text;
            Properties.Settings.Default.opacity = this.Opacity;
            Properties.Settings.Default.canresize = this.ResizeMode != ResizeMode.NoResize;
            Properties.Settings.Default.topmost = this.Topmost;
            Properties.Settings.Default.showhelp = helptxt.Visibility == Visibility.Visible;
            Properties.Settings.Default.Save();
        }

        private bool drag_enabled = false;

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    helptxt.Visibility = helptxt.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
                    break;
                case Key.F2:
                    if ((this.Opacity += 0.05) > 1) this.Opacity = 1;
                    break;
                case Key.F3:
                    if ((this.Opacity -= 0.05) < 0) this.Opacity = 0;
                    break;
                case Key.F5:
                    web.ReloadCommand.Execute(this);
                    break;
                case Key.F6:    //Navigate
                    url.Visibility = Visibility.Visible;
                    url.Focus();
                    break;
                case Key.F8:
                    this.Close();
                    break;
                case Key.F9:
                    this.Topmost = !this.Topmost;
                    break;
                case Key.F11:
                    drag_enabled = !drag_enabled;
                    break;
                case Key.F12:   //Lock
                    this.ResizeMode = this.ResizeMode == ResizeMode.NoResize ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
                    break;
            }
        }

        private void url_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    url.Visibility = Visibility.Hidden;
                    web.Address = url.Text;
                    break;
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (drag_enabled)
            {
                DragMove();
                e.Handled = true;
            }
        }
    }
}
