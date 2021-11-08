using MinatoProject.Apps.ToDoCoreWpf.Content.Extensions;
using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    /// <summary>
    /// TasksPage.xamlのViewModelクラス
    /// </summary>
    public class TasksPageViewModel : BindableBase
    {
        /// <summary>
        /// タスク表示用
        /// </summary>
        public class FilteredTask : ToDoTask
        {
            private ToDoCategory _category;
            /// <summary>
            /// 区分
            /// </summary>
            public ToDoCategory Category
            {
                get => _category;
                set
                {
                    if (value == _category)
                    {
                        return;
                    }
                    _category = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(Category));
                }
            }

            private ToDoStatus _status;
            /// <summary>
            /// 状況
            /// </summary>
            public ToDoStatus Status
            {
                get => _status;
                set
                {
                    if (value == _status)
                    {
                        return;
                    }
                    _status = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(Status));
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="task"></param>
            /// <param name="categories"></param>
            /// <param name="statuses"></param>
            public FilteredTask(ToDoTask task, IList<ToDoCategory> categories, IList<ToDoStatus> statuses) : base(task)
            {
                Category = categories.FirstOrDefault(item => item.Guid.Equals(CategoryGuid));
                Status = statuses.FirstOrDefault(item => item.Guid.Equals(StatusGuid));
            }
        }

        #region プロパティ
        private ObservableCollection<ToDoCategory> _categories = new();
        /// <summary>
        /// 区分一覧
        /// </summary>
        public ObservableCollection<ToDoCategory> Categories
        {
            get => _categories;
            set => _ = SetProperty(ref _categories, value);
        }

        private ObservableCollection<ToDoStatus> _statuses = new();
        /// <summary>
        /// 状況一覧
        /// </summary>
        public ObservableCollection<ToDoStatus> Statuses
        {
            get => _statuses;
            set => _ = SetProperty(ref _statuses, value);
        }

        private ObservableCollection<ToDoTask> _tasks = new();
        /// <summary>
        /// タスク一覧
        /// </summary>
        public ObservableCollection<ToDoTask> Tasks
        {
            get => _tasks;
            set
            {
                _ = SetProperty(ref _tasks, value);
                RaisePropertyChanged(nameof(FilteredTasks));
            }
        }

        /// <summary>
        /// 検索文字列でフィルターしたタスク一覧
        /// </summary>
        public ObservableCollection<FilteredTask> FilteredTasks
        {
            get
            {
                var tasks = new ObservableCollection<FilteredTask>(
                    from item in Tasks select new FilteredTask(item, Categories, Statuses))
                    .SortCollection();

                if (string.IsNullOrEmpty(SearchQuery))
                {
                    return tasks;
                }
                else
                {
                    return new ObservableCollection<FilteredTask>(
                        tasks.Where(item => item.Title.ToLower(CultureInfo.CurrentCulture).Contains(SearchQuery.ToLower(CultureInfo.CurrentCulture)) ||
                                    item.Detail.ToLower(CultureInfo.CurrentCulture).Contains(SearchQuery.ToLower(CultureInfo.CurrentCulture)) ||
                                    item.Category.Name.ToLower(CultureInfo.CurrentCulture).Contains(SearchQuery.ToLower(CultureInfo.CurrentCulture)) ||
                                    item.Status.Name.ToLower(CultureInfo.CurrentCulture).Contains(SearchQuery.ToLower(CultureInfo.CurrentCulture))).ToList())
                        .SortCollection();
                }
            }
        }

        private ToDoTask _selectedTask;
        /// <summary>
        /// 選択されたタスク
        /// </summary>
        public ToDoTask SelectedTask
        {
            get => _selectedTask;
            set => _ = SetProperty(ref _selectedTask, value);
        }

        private string _searchQuery = string.Empty;
        /// <summary>
        /// 検索文字列
        /// </summary>
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _ = SetProperty(ref _searchQuery, value);
                RaisePropertyChanged(nameof(FilteredTasks));
            }
        }
        #endregion

        #region コマンド
        /// <summary>
        /// アプリ終了コマンド
        /// </summary>
        public DelegateCommand ShutdownCommand { get; private set; }
        /// <summary>
        /// タスク作成コマンド
        /// </summary>
        public DelegateCommand CreateNewCommand { get; private set; }
        /// <summary>
        /// 区分設定コマンド
        /// </summary>
        public DelegateCommand ConfigureCategoryCommand { get; private set; }
        /// <summary>
        /// 状況設定コマンド
        /// </summary>
        public DelegateCommand ConfigureStatusCommand { get; private set; }
        /// <summary>
        /// スタイル設定コマンド
        /// </summary>
        public DelegateCommand ConfigureStyleCommand { get; private set; }
        /// <summary>
        /// アプリケーション設定コマンド
        /// </summary>
        public DelegateCommand ApplicationPreferenceCommand { get; private set; }
        /// <summary>
        /// タスク表示コマンド
        /// </summary>
        public DelegateCommand ShowTaskCommand { get; private set; }
        /// <summary>
        /// タスク削除コマンド
        /// </summary>
        public DelegateCommand RemoveTaskCommand { get; private set; }
        #endregion

        #region インターフェイス
        /// <summary>
        /// IDialogService
        /// </summary>
        private readonly IDialogService _dialogService;
        #endregion

        #region メンバ変数
        /// <summary>
        /// 区分一覧ファイルパス
        /// </summary>
        private static readonly string _categoriesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\categories.json";
        /// <summary>
        /// 状況一覧ファイルパス
        /// </summary>
        private static readonly string _statusesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\statuses.json";
        /// <summary>
        /// タスク一覧ファイルパス
        /// </summary>
        private static readonly string _tasksFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\tasks.json";
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// ディスパッチャタイマー
        /// </summary>
        private DispatcherTimer _dispatcherTimer = null;
        /// <summary>
        /// 最後にタスク一覧を更新した日付
        /// </summary>
        private DateTime _lastUpdateDate = DateTime.Now;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">IDialogService</param>
        public TasksPageViewModel(IDialogService dialogService)
        {
            _logger.Info("start");
            // ファイル作成＆読み込み
            if (!File.Exists(_categoriesFilePath))
            {
                File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
            }
            Categories = JsonSerializer.Deserialize<ObservableCollection<ToDoCategory>>(File.ReadAllText(_categoriesFilePath));

            if (!File.Exists(_statusesFilePath))
            {
                File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
            }
            Statuses = JsonSerializer.Deserialize<ObservableCollection<ToDoStatus>>(File.ReadAllText(_statusesFilePath));

            if (!File.Exists(_tasksFilePath))
            {
                File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
            }
            Tasks = JsonSerializer.Deserialize<ObservableCollection<ToDoTask>>(File.ReadAllText(_tasksFilePath));

            // インターフェイスの登録
            _dialogService = dialogService;

            // コマンドの登録
            ShutdownCommand = new DelegateCommand(ExecuteShutdownCommand);
            CreateNewCommand = new DelegateCommand(ExecuteCreateNewCommand);
            ConfigureCategoryCommand = new DelegateCommand(ExecuteConfigureCategoryCommand);
            ConfigureStatusCommand = new DelegateCommand(ExecuteConfigureStatusCommand);
            ConfigureStyleCommand = new DelegateCommand(ExecuteConfigureStyleCommand);
            ApplicationPreferenceCommand = new DelegateCommand(ExecuteApplicationPreferenceCommand);
            ShowTaskCommand = new DelegateCommand(ExecuteShowTaskCommand, CanExecuteShowTaskCommand)
                .ObservesProperty(() => SelectedTask);
            RemoveTaskCommand = new DelegateCommand(ExecuteRemoveTaskCommand, CanExecuteRemoveTaskCommand)
                .ObservesProperty(() => SelectedTask);
            _logger.Info("end");

            // ディスパッチャタイマーの開始
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _dispatcherTimer.Tick += OnDispatcherTimerTicked;
            _dispatcherTimer.Start();
        }
        #endregion

        #region ファイナライザ
        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~TasksPageViewModel()
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= OnDispatcherTimerTicked;
        }
        #endregion

        /// <summary>
        /// アプリ終了コマンドを実行する
        /// </summary>
        private void ExecuteShutdownCommand()
        {
            _logger.Info("start");
            Application.Current.Shutdown();
            _logger.Info("end");
        }

        /// <summary>
        /// タスク作成コマンドを実行する
        /// </summary>
        private void ExecuteCreateNewCommand()
        {
            _logger.Info("start");
            var param = new DialogParameters
            {
                { "Task", new ToDoTask() },
            };
            _dialogService.ShowDialog("TaskDialog", param, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var updateTask = r.Parameters.GetValue<ToDoTask>("UpdateTask");
                    UpdateTasks(updateTask);
                }
            });
            SelectedTask = null;
            _logger.Info("end");
        }

        /// <summary>
        /// 区分設定コマンドを実行する
        /// </summary>
        private void ExecuteConfigureCategoryCommand()
        {
            _logger.Info("start");
            var param = new DialogParameters
            {
                { "Categories", Categories.ToList() },
            };
            _dialogService.ShowDialog("ConfigureCategoryDialog", param, null);
            UpdateTasks();
            _logger.Info("end");
        }

        /// <summary>
        /// 状況設定コマンドを実行する
        /// </summary>
        private void ExecuteConfigureStatusCommand()
        {
            _logger.Info("start");
            var param = new DialogParameters
            {
                { "Statuses", Statuses.ToList() },
            };
            _dialogService.ShowDialog("ConfigureStatusDialog", param, null);
            UpdateTasks();
            _logger.Info("end");
        }

        /// <summary>
        /// スタイル設定コマンドを実行する
        /// </summary>
        private void ExecuteConfigureStyleCommand()
        {
            _logger.Info("start");
            var param = new DialogParameters
            {
                { "Categories", Categories.ToList() },
                { "Statuses", Statuses.ToList() },
            };
            _dialogService.ShowDialog("ConfigureStyleDialog", param, null);
            UpdateTasks();
            _logger.Info("end");
        }

        /// <summary>
        /// アプリケーション設定コマンドを実行する
        /// </summary>
        private void ExecuteApplicationPreferenceCommand()
        {
            _logger.Info("start");
            _dialogService.ShowDialog("ApplicationPreferenceDialog", null, null);
            _logger.Info("end");
        }

        /// <summary>
        /// タスク表示コマンドを実行する
        /// </summary>
        private void ExecuteShowTaskCommand()
        {
            _logger.Info("start");
            var param = new DialogParameters
            {
                { "Task", new ToDoTask(SelectedTask) },
            };
            _dialogService.ShowDialog("TaskDialog", param, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var updateTask = r.Parameters.GetValue<ToDoTask>("UpdateTask");
                    UpdateTasks(updateTask);
                }
            });
            SelectedTask = null;
            _logger.Info("end");
        }
        /// <summary>
        /// タスク作成コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteShowTaskCommand()
        {
            return SelectedTask != null;
        }

        /// <summary>
        /// タスク削除コマンドを実行する
        /// </summary>
        private void ExecuteRemoveTaskCommand()
        {
            _logger.Info("start");
            _ = Tasks.Remove(SelectedTask);
            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
            Tasks = Tasks.SortCollection();
            _logger.Info("end");
        }
        /// <summary>
        /// タスク削除コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteRemoveTaskCommand()
        {
            return SelectedTask != null;
        }

        /// <summary>
        /// タスク一覧を更新する
        /// </summary>
        /// <param name="targetTask"></param>
        private void UpdateTasks(ToDoTask targetTask = null)
        {
            _logger.Info("start");

            Categories = JsonSerializer.Deserialize<ObservableCollection<ToDoCategory>>(File.ReadAllText(_categoriesFilePath))
                .SortCollection();
            Statuses = JsonSerializer.Deserialize<ObservableCollection<ToDoStatus>>(File.ReadAllText(_statusesFilePath))
                .SortCollection();

            if (targetTask == null)
            {
                // タスクが渡っていなかったらファイルを読み直す
                Tasks = JsonSerializer.Deserialize<ObservableCollection<ToDoTask>>(File.ReadAllText(_tasksFilePath))
                    .SortCollection();
            }
            else
            {
                // タスクを更新
                var previousTask = Tasks.FirstOrDefault(item => item.Equals(targetTask));
                if (previousTask != null)
                {
                    _ = Tasks.Remove(previousTask);
                }
                Tasks.Add(targetTask);
                Tasks = Tasks.SortCollection();
                File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
            }

            // 外部キーを更新
            foreach (var task in Tasks)
            {
                var category = Categories.FirstOrDefault(item => item.Guid.Equals(task.CategoryGuid));
                if (category == null)
                {
                    task.CategoryGuid = default;
                }

                var status = Statuses.FirstOrDefault(item => item.Guid.Equals(task.StatusGuid));
                if (status == null)
                {
                    task.StatusGuid = default;
                }
            }
            Tasks = Tasks.SortCollection();
            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));

            _logger.Info("end");
        }

        /// <summary>
        /// DispatcherTimerのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDispatcherTimerTicked(object sender, EventArgs e)
        {
            var current = DateTime.Now;
            if (_lastUpdateDate.Date < current.Date)
            {
                _logger.Info("detected date changed");
                UpdateTasks();
                _lastUpdateDate = current;
            }
        }
    }
}
