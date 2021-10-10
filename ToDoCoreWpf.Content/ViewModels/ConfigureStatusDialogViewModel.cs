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
    /// ConfigureStatusDialog.xamlのViewModelクラス
    /// </summary>
    public class ConfigureStatusDialogViewModel : BindableBase, IDialogAware
    {
        #region プロパティ
        /// <summary>
        /// 状況一覧（表示用）
        /// </summary>
        public ObservableCollection<ToDoStatus> DisplayStatuses
        {
            get
            {
                var list = new List<ToDoStatus>(Statuses);
                list.Sort();
                return new ObservableCollection<ToDoStatus>(list);
            }
        }

        private List<ToDoStatus> _statuses = new();
        /// <summary>
        /// 状況一覧
        /// </summary>
        public List<ToDoStatus> Statuses
        {
            get => _statuses;
            set
            {
                _ = SetProperty(ref _statuses, value);
                RaisePropertyChanged(nameof(DisplayStatuses));
            }
        }

        private ToDoStatus _selectedStatus;
        /// <summary>
        /// 選択された状況
        /// </summary>
        public ToDoStatus SelectedStatus
        {
            get => _selectedStatus;
            set => _ = SetProperty(ref _selectedStatus, value);
        }

        private ToDoStatus _newStatus = new();
        /// <summary>
        /// 新しい状況
        /// </summary>
        public ToDoStatus NewStatus
        {
            get => _newStatus;
            set => _ = SetProperty(ref _newStatus, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// 追加コマンド
        /// </summary>
        public DelegateCommand AddCommand { get; private set; }
        /// <summary>
        /// 削除コマンド
        /// </summary>
        public DelegateCommand RemoveCommand { get; private set; }
        /// <summary>
        /// 上へコマンド
        /// </summary>
        public DelegateCommand UpCommand { get; private set; }
        /// <summary>
        /// 下へコマンド
        /// </summary>
        public DelegateCommand DownCommand { get; private set; }
        #endregion

        #region IDialogAware
        /// <summary>
        /// ダイアログのタイトル
        /// </summary>
        public string Title => "Configure Status";

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
            Statuses = parameters.GetValue<List<ToDoStatus>>("Statuses");
            _logger.Info("end");
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 状況一覧のファイルパス
        /// </summary>
        private static readonly string _statusesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\statuses.json";
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigureStatusDialogViewModel()
        {
            _logger.Info("start");
            // コマンドの登録
            AddCommand = new DelegateCommand(ExecuteAddCommand, CanExecuteAddCommand)
                .ObservesProperty(() => NewStatus);
            RemoveCommand = new DelegateCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand)
                .ObservesProperty(() => SelectedStatus);
            UpCommand = new DelegateCommand(ExecuteUpCommand, CanExecuteUpCommand)
                .ObservesProperty(() => SelectedStatus);
            DownCommand = new DelegateCommand(ExecuteDownCommand, CanExecuteDownCommand)
                .ObservesProperty(() => SelectedStatus);
            _logger.Info("end");
        }
        #endregion

        /// <summary>
        /// 追加コマンドを実行する
        /// </summary>
        private void ExecuteAddCommand()
        {
            _logger.Info("start");
            int order = Statuses.Count == 0 ? 0 : Statuses.Max(item => item.Order) + 1;
            NewStatus.Order = order;
            Statuses.Add(NewStatus);
            File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
            SelectedStatus = null;
            NewStatus = new ToDoStatus();
            RaisePropertyChanged(nameof(DisplayStatuses));
            _logger.Info("end");
        }
        /// <summary>
        /// 追加コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteAddCommand()
        {
            return NewStatus != null &&
                !string.IsNullOrEmpty(NewStatus.Name);
        }

        /// <summary>
        /// 削除コマンドを実行する
        /// </summary>
        private void ExecuteRemoveCommand()
        {
            _logger.Info("start");
            _ = Statuses.Remove(SelectedStatus);
            File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
            RaisePropertyChanged(nameof(DisplayStatuses));
            _logger.Info("end");
        }
        /// <summary>
        /// 削除コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteRemoveCommand()
        {
            return SelectedStatus != null;
        }

        /// <summary>
        /// 上へコマンドを実行する
        /// </summary>
        private void ExecuteUpCommand()
        {
            _logger.Info("start");
            int order = SelectedStatus.Order;
            var item = Statuses.FirstOrDefault(item => item.Order == order - 1);
            item.Order = order;
            SelectedStatus.Order = order - 1;

            File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
            RaisePropertyChanged(nameof(SelectedStatus));
            RaisePropertyChanged(nameof(DisplayStatuses));
            _logger.Info("end");
        }
        /// <summary>
        /// 上へコマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteUpCommand()
        {
            return SelectedStatus != null &&
                SelectedStatus.Order > 0;
        }

        /// <summary>
        /// 下へコマンドを実行する
        /// </summary>
        private void ExecuteDownCommand()
        {
            _logger.Info("start");
            int order = SelectedStatus.Order;
            var item = Statuses.FirstOrDefault(item => item.Order == order + 1);
            item.Order = order;
            SelectedStatus.Order = order + 1;

            File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
            RaisePropertyChanged(nameof(SelectedStatus));
            RaisePropertyChanged(nameof(DisplayStatuses));
            _logger.Info("end");
        }
        /// <summary>
        /// 下へコマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteDownCommand()
        {
            return SelectedStatus != null &&
                SelectedStatus.Order < Statuses.Count - 1;
        }
    }
}
