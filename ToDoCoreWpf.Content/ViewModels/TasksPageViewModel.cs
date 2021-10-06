using MinatoProject.Apps.ToDoCoreWpf.Content.Extensions;
using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    public class TasksPageViewModel : BindableBase
    {
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
        public ObservableCollection<ToDoTask> FilteredTasks
        {
            get
            {
                if (string.IsNullOrEmpty(SearchQuery))
                {
                    return Tasks.SortCollection();
                }
                else
                {
                    return new ObservableCollection<ToDoTask>(
                        Tasks.Where(item => item.Title.ToLower(CultureInfo.CurrentCulture).Contains(SearchQuery.ToLower(CultureInfo.CurrentCulture)) ||
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
        /// 
        /// </summary>
        public DelegateCommand ShutdownCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand CreateNewCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand ConfigureCategoryCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand ConfigureStatusCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand ConfigureStyleCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand ShowTaskCommand { get; private set; }
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        private static readonly string _categoriesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\categories.json";
        /// <summary>
        /// 
        /// </summary>
        private static readonly string _statusesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\statuses.json";
        /// <summary>
        /// 
        /// </summary>
        private static readonly string _tasksFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\tasks.json";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">IDialogService</param>
        public TasksPageViewModel(IDialogService dialogService)
        {
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
            ShowTaskCommand = new DelegateCommand(ExecuteShowTaskCommand, CanExecuteShowTaskCommand)
                .ObservesProperty(() => SelectedTask);
            RemoveTaskCommand = new DelegateCommand(ExecuteRemoveTaskCommand, CanExecuteRemoveTaskCommand)
                .ObservesProperty(() => SelectedTask);
        }
        #endregion

        private void ExecuteShutdownCommand()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteCreateNewCommand()
        {
            var param = new DialogParameters
            {
                { "Categories", Categories },
                { "Statuses", Statuses },
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
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteConfigureCategoryCommand()
        {
            var param = new DialogParameters
            {
                { "Categories", Categories.ToList() },
            };
            _dialogService.ShowDialog("ConfigureCategoryDialog", param, null);
            Categories = JsonSerializer.Deserialize<ObservableCollection<ToDoCategory>>(File.ReadAllText(_categoriesFilePath));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteConfigureStatusCommand()
        {
            var param = new DialogParameters
            {
                { "Statuses", Statuses.ToList() },
            };
            _dialogService.ShowDialog("ConfigureStatusDialog", param, null);
            Statuses = JsonSerializer.Deserialize<ObservableCollection<ToDoStatus>>(File.ReadAllText(_statusesFilePath));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteConfigureStyleCommand()
        {
            var param = new DialogParameters
            {
                { "Categories", Categories.ToList() },
                { "Statuses", Statuses.ToList() },
            };
            _dialogService.ShowDialog("ConfigureStyleDialog", param, null);
            Categories = JsonSerializer.Deserialize<ObservableCollection<ToDoCategory>>(File.ReadAllText(_categoriesFilePath));
            Statuses = JsonSerializer.Deserialize<ObservableCollection<ToDoStatus>>(File.ReadAllText(_statusesFilePath));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteShowTaskCommand()
        {
            var param = new DialogParameters
            {
                { "Categories", Categories },
                { "Statuses", Statuses },
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteShowTaskCommand()
        {
            return SelectedTask != null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteRemoveTaskCommand()
        {
            _ = Tasks.Remove(SelectedTask);
            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
        }
        /// <summary>
        /// 
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
        private void UpdateTasks(ToDoTask targetTask)
        {
            // 区分を新規追加
            if (Categories.FirstOrDefault(item => item.Equals(targetTask.Category)) == null)
            {
                Categories.Add(targetTask.Category);
                Categories = Categories.SortCollection();
                File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
            }

            // 状況を新規追加
            if (Statuses.FirstOrDefault(item => item.Equals(targetTask.Status)) == null)
            {
                Statuses.Add(targetTask.Status);
                Statuses = Statuses.SortCollection();
                File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
            }

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
    }
}
