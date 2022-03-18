using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

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
            set
            {
                var list = new List<ToDoCategory>(value);
                list.Sort();
                _ = SetProperty(ref _categories, new ObservableCollection<ToDoCategory>(list));
            }
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

        private string _selectedCategoryValue = string.Empty;
        /// <summary>
        /// 区分に表示されている文字列
        /// </summary>
        public string SelectedCategoryValue
        {
            get => _selectedCategoryValue;
            set => _ = SetProperty(ref _selectedCategoryValue, value);
        }

        private string _selectedStatusValue = string.Empty;
        /// <summary>
        /// 状況に表示されている文字列
        /// </summary>
        public string SelectedStatusValue
        {
            get => _selectedStatusValue;
            set => _ = SetProperty(ref _selectedStatusValue, value);
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
            Categories = JsonSerializer.Deserialize<ObservableCollection<ToDoCategory>>(File.ReadAllText(_categoriesFilePath));
            Statuses = JsonSerializer.Deserialize<ObservableCollection<ToDoStatus>>(File.ReadAllText(_statusesFilePath));
            Task = parameters.GetValue<ToDoTask>("Task");
            if (Task != null && Task.Guid != default)
            {
                if (Task.CategoryGuid != default)
                {
                    _currentCategory = new ToDoCategory(Categories.FirstOrDefault(item => item.Guid.Equals(Task.CategoryGuid)));
                    SelectedCategoryValue = _currentCategory.Name;
                }

                if (Task.StatusGuid != default)
                {
                    _currentStatus = new ToDoStatus(Statuses.FirstOrDefault(item => item.Guid.Equals(Task.StatusGuid)));
                    SelectedStatusValue = _currentStatus.Name;
                }
            }
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
        /// <summary>
        /// 区分一覧ファイルパス
        /// </summary>
        private static readonly string _categoriesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\categories.json";
        /// <summary>
        /// 状況一覧ファイルパス
        /// </summary>
        private static readonly string _statusesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\statuses.json";
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
            // 区分と状況の名前が変更されていたら新しいGuidに差し替える
            if (!string.IsNullOrEmpty(SelectedCategoryValue))
            {
                var updateCategory = Categories.FirstOrDefault(item => item.Name.Equals(SelectedCategoryValue));
                if (updateCategory == null)
                {
                    // 新規作成
                    int order = Categories.Count == 0 ? 0 : Categories.Max(item => item.Order) + 1;
                    var category = new ToDoCategory()
                    {
                        Order = order,
                        Name = SelectedCategoryValue
                    };
                    Categories.Add(category);
                    File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));

                    Task.CategoryGuid = category.Guid;
                }
                else
                {
                    Task.CategoryGuid = updateCategory.Guid;
                }
            }

            if (!string.IsNullOrEmpty(SelectedStatusValue))
            {
                var updateStatus = Statuses.FirstOrDefault(item => item.Name.Equals(SelectedStatusValue));
                if (updateStatus == null)
                {
                    // 新規作成
                    int order = Statuses.Count == 0 ? 0 : Statuses.Max(item => item.Order) + 1;
                    var status = new ToDoStatus()
                    {
                        Order = order,
                        Name = SelectedStatusValue
                    };
                    Statuses.Add(status);
                    File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));

                    Task.StatusGuid = status.Guid;
                }
                else
                {
                    Task.StatusGuid = updateStatus.Guid;
                }
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
