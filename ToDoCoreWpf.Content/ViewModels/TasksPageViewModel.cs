using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
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
    public class TasksPageViewModel : BindableBase
    {
        #region プロパティ
        private ObservableCollection<ToDoTask> _tasks = new ObservableCollection<ToDoTask>();
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ToDoTask> Tasks
        {
            get => _tasks;
            set => _ = SetProperty(ref _tasks, value);
        }

        private ToDoTask _selectedTask;
        /// <summary>
        /// 
        /// </summary>
        public ToDoTask SelectedTask
        {
            get => _selectedTask;
            set => _ = SetProperty(ref _selectedTask, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand ShowTaskCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand RemoveTaskCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand OnSourceUpdatedCommand { get; private set; }
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
        private static readonly string _tasksFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\tasks.json";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">IDialogService</param>
        public TasksPageViewModel(IDialogService dialogService)
        {
            if (!File.Exists(_tasksFilePath))
            {
                File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
            }

            Tasks = JsonSerializer.Deserialize<ObservableCollection<ToDoTask>>(File.ReadAllText(_tasksFilePath));

            // インターフェイスの登録
            _dialogService = dialogService;

            // コマンドの登録
            ShowTaskCommand = new DelegateCommand(ExecuteShowTaskCommand, CanExecuteShowTaskCommand)
                .ObservesProperty(() => SelectedTask);
            RemoveTaskCommand = new DelegateCommand(ExecuteRemoveTaskCommand, CanExecuteRemoveTaskCommand)
                .ObservesProperty(() => SelectedTask);
            OnSourceUpdatedCommand = new DelegateCommand(ExecuteOnSourceUpdatedCommand);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOnSourceUpdatedCommand()
        {
            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
        }

        private void ExecuteShowTaskCommand()
        {
            var ret = ButtonResult.Cancel;
            _dialogService.ShowDialog("TaskDialog", null, r => ret = r.Result, "");
        }
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
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private ObservableCollection<ToDoTask> SortTasks(ObservableCollection<ToDoTask> source)
        {
            var taskList = source.ToList();
            taskList.Sort();
            return new ObservableCollection<ToDoTask>(taskList);
        }
    }
}
