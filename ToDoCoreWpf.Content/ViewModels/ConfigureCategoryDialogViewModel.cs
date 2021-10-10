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
    /// ConfigureCategoryDialog.xamlのViewModelクラス
    /// </summary>
    public class ConfigureCategoryDialogViewModel : BindableBase, IDialogAware
    {
        #region プロパティ
        /// <summary>
        /// 区分一覧（表示用）
        /// </summary>
        public ObservableCollection<ToDoCategory> DisplayCategories
        {
            get
            {
                var list = new List<ToDoCategory>(Categories);
                list.Sort();
                return new ObservableCollection<ToDoCategory>(list);
            }
        }

        private List<ToDoCategory> _categories = new();
        /// <summary>
        /// 区分一覧
        /// </summary>
        public List<ToDoCategory> Categories
        {
            get => _categories;
            set
            {
                _ = SetProperty(ref _categories, value);
                RaisePropertyChanged(nameof(DisplayCategories));
            }
        }

        private ToDoCategory _selectedCategory;
        /// <summary>
        /// 選択された区分
        /// </summary>
        public ToDoCategory SelectedCategory
        {
            get => _selectedCategory;
            set => _ = SetProperty(ref _selectedCategory, value);
        }

        private ToDoCategory _newCategory = new();
        /// <summary>
        /// 新しい区分
        /// </summary>
        public ToDoCategory NewCategory
        {
            get => _newCategory;
            set => _ = SetProperty(ref _newCategory, value);
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
        public string Title => "Configure Category";

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
            Categories = parameters.GetValue<List<ToDoCategory>>("Categories");
            _logger.Info("end");
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 区分一覧のファイルパス
        /// </summary>
        private static readonly string _categoriesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\categories.json";
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigureCategoryDialogViewModel()
        {
            _logger.Info("start");
            // コマンドの登録
            AddCommand = new DelegateCommand(ExecuteAddCommand, CanExecuteAddCommand)
                .ObservesProperty(() => NewCategory);
            RemoveCommand = new DelegateCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand)
                .ObservesProperty(() => SelectedCategory);
            UpCommand = new DelegateCommand(ExecuteUpCommand, CanExecuteUpCommand)
                .ObservesProperty(() => SelectedCategory);
            DownCommand = new DelegateCommand(ExecuteDownCommand, CanExecuteDownCommand)
                .ObservesProperty(() => SelectedCategory);
            _logger.Info("end");
        }
        #endregion

        /// <summary>
        /// 追加コマンドを実行する
        /// </summary>
        private void ExecuteAddCommand()
        {
            _logger.Info("start");
            int order = Categories.Count == 0 ? 0 : Categories.Max(item => item.Order) + 1;
            NewCategory.Order = order;
            Categories.Add(NewCategory);
            File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
            SelectedCategory = null;
            NewCategory = new ToDoCategory();
            RaisePropertyChanged(nameof(DisplayCategories));
            _logger.Info("end");
        }
        /// <summary>
        /// 追加コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteAddCommand()
        {
            return NewCategory != null &&
                !string.IsNullOrEmpty(NewCategory.Name);
        }

        /// <summary>
        /// 削除コマンドを実行する
        /// </summary>
        private void ExecuteRemoveCommand()
        {
            _logger.Info("start");
            _ = Categories.Remove(SelectedCategory);
            File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
            RaisePropertyChanged(nameof(DisplayCategories));
            _logger.Info("end");
        }
        /// <summary>
        /// 削除コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteRemoveCommand()
        {
            return SelectedCategory != null;
        }

        /// <summary>
        /// 上へコマンドを実行する
        /// </summary>
        private void ExecuteUpCommand()
        {
            _logger.Info("start");
            int order = SelectedCategory.Order;
            var item = Categories.FirstOrDefault(item => item.Order == order - 1);
            item.Order = order;
            SelectedCategory.Order = order - 1;

            File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
            RaisePropertyChanged(nameof(SelectedCategory));
            RaisePropertyChanged(nameof(DisplayCategories));
            _logger.Info("end");
        }
        /// <summary>
        /// 上へコマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteUpCommand()
        {
            return SelectedCategory != null &&
                SelectedCategory.Order > 0;
        }

        /// <summary>
        /// 下へコマンドを実行する
        /// </summary>
        private void ExecuteDownCommand()
        {
            _logger.Info("start");
            int order = SelectedCategory.Order;
            var item = Categories.FirstOrDefault(item => item.Order == order + 1);
            item.Order = order;
            SelectedCategory.Order = order + 1;

            File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
            RaisePropertyChanged(nameof(SelectedCategory));
            RaisePropertyChanged(nameof(DisplayCategories));
            _logger.Info("end");
        }
        /// <summary>
        /// 下へコマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteDownCommand()
        {
            return SelectedCategory != null &&
                SelectedCategory.Order < Categories.Count - 1;
        }
    }
}
