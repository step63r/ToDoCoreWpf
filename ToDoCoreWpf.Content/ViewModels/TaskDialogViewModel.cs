using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
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

        private ObservableCollection<ToDoCategory> _categories;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ToDoCategory> Categories
        {
            get => _categories;
            set => _ = SetProperty(ref _categories, value);
        }

        private ObservableCollection<ToDoStatus> _statuses;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ToDoStatus> Statuses
        {
            get => _statuses;
            set => _ = SetProperty(ref _statuses, value);
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

        #region メンバ変数
        /// <summary>
        /// 
        /// </summary>
        private ToDoCategory _currentCategory;
        /// <summary>
        /// 
        /// </summary>
        private ToDoStatus _currentStatus;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskDialogViewModel()
        {
            // コマンドの登録
            OkCommand = new DelegateCommand(ExecuteOkCommand, CanExecuteOkCommand)
                .ObservesProperty(() => Task.Title);
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOkCommand()
        {
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
            if (Task != null)
            {
                _currentCategory = new ToDoCategory(Task.Category);
                _currentStatus = new ToDoStatus(Task.Status);
            }

            Categories = parameters.GetValue<ObservableCollection<ToDoCategory>>("Categories");
            Statuses = parameters.GetValue<ObservableCollection<ToDoStatus>>("Statuses");
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
