using MinatoProject.Apps.ToDoCoreWpf.Content.Events;
using NLog;
using Prism.Events;
using Prism.Mvvm;
using System;

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

        private string _overdue = string.Empty;
        /// <summary>
        /// ツールチップに表示する文字列（期限超過）
        /// </summary>
        public string Overdue
        {
            get { return _overdue; }
            set { SetProperty(ref _overdue, value); }
        }

        private string _deadline = string.Empty;
        /// <summary>
        /// ツールチップに表示する文字列（本日期限）
        /// </summary>
        public string Deadline
        {
            get { return _deadline; }
            set { SetProperty(ref _deadline, value); }
        }

        private string _future = string.Empty;
        /// <summary>
        /// ツールチップに表示する文字列（期限前）
        /// </summary>
        public string Future
        {
            get { return _future; }
            set { SetProperty(ref _future, value); }
        }

        private string _odfMessage = string.Empty;
        /// <summary>
        /// ツールチップに表示する文字列
        /// </summary>
        /// <remarks>
        /// <para>Windows11での問題に対する回避策</para>
        /// <para>https://github.com/hardcodet/wpf-notifyicon/issues/65</para>
        /// </remarks>
        public string OdfMessage
        {
            get { return _odfMessage; }
            set { SetProperty(ref _odfMessage, value); }
        }
        #endregion

        #region インターフェイス
        /// <summary>
        /// IEventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator;
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
        /// <param name="eventAggregator">IEventAggregator</param>
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _logger.Info("start");
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<TaskEvent>().Subscribe((parameters) =>
            {
                Overdue = parameters.Overdue == 0 ? string.Empty : $"{parameters.Overdue} overdue tasks";
                Deadline = parameters.Deadline == 0 ? string.Empty : $"{parameters.Deadline} deadline tasks";
                Future = parameters.Future == 0 ? string.Empty : $"{parameters.Future} future tasks";

                // Windows11での問題に対する回避策
                // https://github.com/hardcodet/wpf-notifyicon/issues/65
                OdfMessage = Title + Environment.NewLine;
                OdfMessage += parameters.Overdue == 0 ? string.Empty : $" - {parameters.Overdue} overdue tasks" + Environment.NewLine;
                OdfMessage += parameters.Deadline == 0 ? string.Empty : $" - {parameters.Deadline} deadline tasks" + Environment.NewLine;
                OdfMessage += parameters.Future == 0 ? string.Empty : $" - {parameters.Future} future tasks";
            });
            _logger.Info("end");
        }
        #endregion
    }
}
