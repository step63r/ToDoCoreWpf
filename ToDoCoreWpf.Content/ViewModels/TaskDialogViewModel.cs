using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskDialogViewModel : BindableBase, IDialogAware
    {

        #region IDialogAware
        /// <summary>
        /// IDialogAware
        /// </summary>
        public event Action<IDialogResult> RequestClose;
        #endregion

        #region プロパティ
        private string _title = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get => _title;
            set => _ = SetProperty(ref _title, value);
        }
        private ToDoTask _task;
        /// <summary>
        /// 
        /// </summary>
        public ToDoTask Task
        {
            get => _task;
            set => _ = SetProperty(ref _task, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand OkCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskDialogViewModel()
        {
            // コマンドの登録
            OkCommand = new DelegateCommand(ExecuteOkCommand, CanExecuteOkCommand)
                .ObservesProperty(() => Task);
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOkCommand()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteOkCommand()
        {
            return !string.IsNullOrEmpty(Task.Title);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteCancelCommand()
        {

        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
