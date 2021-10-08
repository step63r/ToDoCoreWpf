using MinatoProject.Apps.ToDoCoreWpf.Core.Native;
using MinatoProject.Apps.ToDoCoreWpf.Core.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace MinatoProject.Apps.ToDoCoreWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region メンバ変数
        /// <summary>
        /// 設定情報
        /// </summary>
        private readonly SettingsStore _settings;
        /// <summary>
        /// 押下されているキーのリスト
        /// </summary>
        private readonly List<Keys> _detectedKeys = new();
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadWindowBounds();

            // 他のDLLと共有する設定値はここで初期化する
            _settings = SettingsStore.GetInstance();
            _settings.InitializeInstance();

            KeyboardHook.AddEvent(HookKeyboard);
            KeyboardHook.Start();
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
            if (_settings.GetSettings().ExitAsMinimized)
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

                KeyboardHook.RemoveEvent(HookKeyboard);
                KeyboardHook.Stop();
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
            System.Windows.Application.Current.Shutdown();

            KeyboardHook.RemoveEvent(HookKeyboard);
            KeyboardHook.Stop();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        private void HookKeyboard(ref KeyboardHook.StateKeyboard s)
        {
            switch (s.Stroke)
            {
                case KeyboardHook.Stroke.KEY_DOWN:
                case KeyboardHook.Stroke.SYSKEY_DOWN:
                    if (!_detectedKeys.Contains(s.Key))
                    {
                        _detectedKeys.Add(s.Key);
                    }

                    // フックするキーに設定されている全てが押下されているかチェック
                    bool ret = true;
                    foreach (var k in _settings.GetSettings().HookKeys)
                    {
                        if (!_detectedKeys.Contains(k))
                        {
                            ret = false;
                        }
                    }

                    // 条件を満たしたらこのウィンドウを最前面に
                    if (ret)
                    {
                        if (!IsVisible)
                        {
                            Show();
                        }

                        if (WindowState == WindowState.Minimized)
                        {
                            WindowState = WindowState.Normal;
                        }

                        if (!ShowInTaskbar)
                        {
                            taskbarIcon.Visibility = Visibility.Collapsed;
                            ShowInTaskbar = true;
                            WindowState = WindowState.Normal;
                        }

                        _ = Activate();
                        Topmost = true;
                        Topmost = false;
                        _ = Focus();
                    }
                    break;

                case KeyboardHook.Stroke.KEY_UP:
                case KeyboardHook.Stroke.SYSKEY_UP:
                    if (_detectedKeys.Contains(s.Key))
                    {
                        _ = _detectedKeys.Remove(s.Key);
                    }
                    break;

                case KeyboardHook.Stroke.UNKNOWN:
                default:
                    break;
            }
        }
    }
}
