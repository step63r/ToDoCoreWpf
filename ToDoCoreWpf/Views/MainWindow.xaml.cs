using System.Windows;

namespace MinatoProject.Apps.ToDoCoreWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadWindowBounds();
        }

        /// <summary>
        /// ウィンドウが閉じられるときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // ウィンドウの位置・サイズを保存
            SaveWindowBounds();

            // 通知領域に格納する設定なら終了をキャンセル
            if (Properties.Settings.Default.ExitAsMinimized)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;
                ShowInTaskbar = false;
                taskbarIcon.Visibility = Visibility.Visible;
            }
            else
            {
                e.Cancel = false;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// ウィンドウの状態が変化したときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Normal || WindowState == WindowState.Maximized)
            {
                ShowInTaskbar = true;
            }
        }

        /// <summary>
        /// ShowWindowMenuItemクリック時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowWindowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            taskbarIcon.Visibility = Visibility.Collapsed;
            ShowInTaskbar = true;
            WindowState = WindowState.Normal;
        }

        /// <summary>
        /// ExitMenuItemクリック時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// ウィンドウの情報を保存する
        /// </summary>
        private void SaveWindowBounds()
        {
            var settings = Properties.Settings.Default;
            settings.WindowState = WindowState;
            // 最大化を解除する
            WindowState = WindowState.Normal;
            settings.Width = Width;
            settings.Height = Height;
            settings.Top = Top;
            settings.Left = Left;
            settings.Save();
        }

        /// <summary>
        /// ウィンドウの位置・サイズを復元する
        /// </summary>
        private void LoadWindowBounds()
        {
            var settings = Properties.Settings.Default;
            if (settings.Left >= 0 && settings.Left + settings.Width < SystemParameters.VirtualScreenWidth)
            {
                Left = settings.Left;
            }
            if (settings.Top >= 0 && settings.Top + settings.Height < SystemParameters.VirtualScreenHeight)
            {
                Top = settings.Top;
            }
            if (settings.Width > 0 && settings.Width <= SystemParameters.WorkArea.Width)
            {
                Width = settings.Width;
            }
            if (settings.Height > 0 && settings.Height <= SystemParameters.WorkArea.Height)
            {
                Height = settings.Height;
            }
            if (settings.WindowState == WindowState.Maximized)
            {
                Loaded += (sender, e) => WindowState = WindowState.Maximized;
            }
        }
    }
}
