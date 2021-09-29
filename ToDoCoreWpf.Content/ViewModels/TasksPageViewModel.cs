using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    public class TasksPageViewModel : BindableBase
    {
        #region プロパティ
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ToDoTask> Tasks { get; set; } = new ObservableCollection<ToDoTask>();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        public TasksPageViewModel()
        {
            var now = DateTime.Now;
            Tasks = new ObservableCollection<ToDoTask>()
            {
                new ToDoTask()
                {
                    Category = new ToDoCategory(){ Name = "区分１" },
                    Status = new ToDoStatus(){ Name = "実行中" },
                    Created = now,
                    Updated = now,
                    DueDate = now + new TimeSpan(7, 0, 0, 0),
                    Title = "タスク１",
                    Detail = string.Empty,
                },
                new ToDoTask()
                {
                    Category = new ToDoCategory(){ Name = "区分１" },
                    Status = new ToDoStatus(){ Name = "実行中" },
                    Created = now,
                    Updated = now,
                    DueDate = now + new TimeSpan(14, 0, 0, 0),
                    Title = "タスク２",
                    Detail = string.Empty,
                },
                new ToDoTask()
                {
                    Category = new ToDoCategory(){ Name = "区分１" },
                    Status = new ToDoStatus(){ Name = "実行中" },
                    Created = now,
                    Updated = now,
                    DueDate = now.AddMonths(1),
                    Title = "タスク３",
                    Detail = string.Empty,
                },
            };
        }
        #endregion
    }
}
