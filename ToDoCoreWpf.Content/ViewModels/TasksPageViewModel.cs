using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using MinatoProject.Core.Codecs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        #region メンバ変数
        /// <summary>
        /// 
        /// </summary>
        private readonly string _tasksFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\tasks.xml";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        public TasksPageViewModel()
        {
            if (!File.Exists(_tasksFilePath))
            {
                _ = XmlConverter.SerializeAsync(Tasks, _tasksFilePath);
            }

            Tasks = XmlConverter.DeserializeAsync<ObservableCollection<ToDoTask>>(_tasksFilePath).Result;
        }
        #endregion
    }
}
