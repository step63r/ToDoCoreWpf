using NLog;
using Prism.Mvvm;

namespace MinatoProject.Apps.ToDoCoreWpf.ViewModels
{
    /// <summary>
    /// MainWindow.xamlのViewModelクラス
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        #region プロパティ
        private string _title = "ToDo Core Wpf";
        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            _logger.Info("start");
            _logger.Info("end");
        }
        #endregion
    }
}
