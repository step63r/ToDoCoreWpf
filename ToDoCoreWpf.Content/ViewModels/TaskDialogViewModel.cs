using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    /// <summary>
    /// TaskDialog.xamlのViewModelクラス
    /// </summary>
    public class TaskDialogViewModel : BindableBase, IDialogAware
    {
        #region プロパティ
        private ToDoTask _task;
        /// <summary>
        /// タスク
        /// </summary>
        public ToDoTask Task
        {
            get => _task;
            set => _ = SetProperty(ref _task, value);
        }

        private ObservableCollection<ToDoCategory> _categories;
        /// <summary>
        /// 区分一覧
        /// </summary>
        public ObservableCollection<ToDoCategory> Categories
        {
            get => _categories;
            set => _ = SetProperty(ref _categories, value);
        }

        private ObservableCollection<ToDoStatus> _statuses;
        /// <summary>
        /// 状況一覧
        /// </summary>
        public ObservableCollection<ToDoStatus> Statuses
        {
            get => _statuses;
            set => _ = SetProperty(ref _statuses, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// OKコマンド
        /// </summary>
        public DelegateCommand OkCommand { get; private set; }
        /// <summary>
        /// Cancelコマンド
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }
        #endregion

        #region IDialogAware
        /// <summary>
        /// ダイアログのタイトル
        /// </summary>
        public string Title => Task?.Title;

        /// <summary>
        /// ダイアログ結果
        /// </summary>
        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// ダイアログを閉じることができるかどうかを判定する
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// ダイアログが閉じたときのイベントハンドラ
        /// </summary>
        public void OnDialogClosed()
        {
            _logger.Info("start");
            _logger.Info("end");
        }

        /// <summary>
        /// ダイアログが開いたときのイベントハンドラ
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            _logger.Info("start");
            Task = parameters.GetValue<ToDoTask>("Task");
            if (Task != null)
            {
                _currentCategory = new ToDoCategory(Task.Category);
                _currentStatus = new ToDoStatus(Task.Status);
            }

            Categories = parameters.GetValue<ObservableCollection<ToDoCategory>>("Categories");
            Statuses = parameters.GetValue<ObservableCollection<ToDoStatus>>("Statuses");
            _logger.Info("end");
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 現在の区分
        /// </summary>
        private ToDoCategory _currentCategory;
        /// <summary>
        /// 現在の状況
        /// </summary>
        private ToDoStatus _currentStatus;
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskDialogViewModel()
        {
            _logger.Info("start");
            // コマンドの登録
            OkCommand = new DelegateCommand(ExecuteOkCommand, CanExecuteOkCommand)
                .ObservesProperty(() => Task.Title);
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);
            _logger.Info("end");
        }
        #endregion

        /// <summary>
        /// OKコマンドを実行する
        /// </summary>
        private void ExecuteOkCommand()
        {
            _logger.Info("start");
            // 区分と状況の名前が変更されていたら新しいインスタンスに差し替える
            if (!_currentCategory.Name.Equals(Task.Category.Name))
            {
                var category = Categories.FirstOrDefault(item => item.Name.Equals(Task.Category.Name));
                if (category == null)
                {
                    int order = Categories.Count == 0 ? 0 : Categories.Max(item => item.Order) + 1;
                    category = new ToDoCategory()
                    {
                        Order = order,
                        Name = Task.Category.Name
                    };
                }
                Task.Category = category;
            }

            if (!_currentStatus.Name.Equals(Task.Status.Name))
            {
                var status = Statuses.FirstOrDefault(item => item.Name.Equals(Task.Status.Name));
                if (status == null)
                {
                    int order = Statuses.Count == 0 ? 0 : Statuses.Max(item => item.Order) + 1;
                    status = new ToDoStatus()
                    {
                        Order = order,
                        Name = Task.Status.Name
                    };
                }
                Task.Status = status;
            }

            Task.Updated = DateTime.Now;

            var param = new DialogParameters()
            {
                { "UpdateTask", Task }
            };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
            _logger.Info("end");
        }
        /// <summary>
        /// OKコマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteOkCommand()
        {
            return Task != null && !string.IsNullOrEmpty(Task.Title);
        }

        /// <summary>
        /// Cancelコマンドを実行する
        /// </summary>
        private void ExecuteCancelCommand()
        {
            _logger.Info("start");
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            _logger.Info("end");
        }
    }
}
