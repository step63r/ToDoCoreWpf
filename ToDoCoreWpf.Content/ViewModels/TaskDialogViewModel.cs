using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

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
        /// <summary>
        /// 
        /// </summary>
        public string Title => Task?.Title;

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
            var param = new DialogParameters()
            {
                { "UpdateTask", Task }
            };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteOkCommand()
        {
            return Task != null && !string.IsNullOrEmpty(Task.Title);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteCancelCommand()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Task = parameters.GetValue<ToDoTask>("Task");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDialogClosed()
        {

        }
    }
}
