using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using Prism.Commands;
using Prism.Mvvm;
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
        #endregion

        #region コマンド
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand OnSourceUpdatedCommand { get; private set; }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 
        /// </summary>
        private readonly string _tasksFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\tasks.json";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TasksPageViewModel()
        {
            if (!File.Exists(_tasksFilePath))
            {
                File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
            }

            Tasks = JsonSerializer.Deserialize<ObservableCollection<ToDoTask>>(File.ReadAllText(_tasksFilePath));

            // コマンドの登録
            OnSourceUpdatedCommand = new DelegateCommand(ExecuteOnSourceUpdatedCommand);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOnSourceUpdatedCommand()
        {
            File.WriteAllText(_tasksFilePath, JsonSerializer.Serialize(Tasks));
        }
        #endregion
    }
}
