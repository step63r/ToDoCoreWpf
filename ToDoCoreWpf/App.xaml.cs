using Prism.Ioc;
using System.Windows;
using MinatoProject.Apps.ToDoCoreWpf.Views;
using System.Threading;
using Prism.Modularity;
using MinatoProject.Apps.ToDoCoreWpf.Content;
using System;
using System.IO;

namespace MinatoProject.Apps.ToDoCoreWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region メンバ変数
        /// <summary>
        /// 設定ファイルフォルダパス
        /// </summary>
        private static readonly string _settingsPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf";
        /// <summary>
        /// 二重起動防止のミューテックス
        /// </summary>
        private Mutex _mutex = new(false, "MinatoProject.Apps.ToDoCoreWpf");
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            if (!Directory.Exists(_settingsPath))
            {
                _ = Directory.CreateDirectory(_settingsPath);
            }

            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            _ = moduleCatalog.AddModule<ContentModule>();
        }

        /// <summary>
        /// アプリケーション起動時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrismApplication_Startup(object sender, StartupEventArgs e)
        {
            if (_mutex.WaitOne(0, false))
            {
                return;
            }

            _ = MessageBox.Show("アプリケーションは既に起動しています。", "ToDoCoreWpf", MessageBoxButton.OK, MessageBoxImage.Information);
            _mutex.Close();
            _mutex = null;
            Shutdown();
        }

        /// <summary>
        /// アプリケーション終了時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrismApplication_Exit(object sender, ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Close();
            }
        }
    }
}
