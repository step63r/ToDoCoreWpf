using MinatoProject.Apps.ToDoCoreWpf.Content.Models;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Media;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    public class ConfigureStyleDialogViewModel : BindableBase, IDialogAware
    {
        #region プロパティ
        private List<ToDoCategory> _categories = new();
        /// <summary>
        /// 区分一覧
        /// </summary>
        public List<ToDoCategory> Categories
        {
            get => _categories;
            set => _ = SetProperty(ref _categories, value);
        }

        private List<ToDoStatus> _statuses = new();
        /// <summary>
        /// 状況一覧
        /// </summary>
        public List<ToDoStatus> Statuses
        {
            get => _statuses;
            set => _ = SetProperty(ref _statuses, value);
        }

        private ToDoCategory _selectedCategory;
        /// <summary>
        /// 選択された区分
        /// </summary>
        public ToDoCategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                RaisePropertyChanged(nameof(SelectedCategoryForeground));
                RaisePropertyChanged(nameof(SelectedCategoryBackground));
            }
        }

        private ToDoStatus _selectedStatus;
        /// <summary>
        /// 選択された状況
        /// </summary>
        public ToDoStatus SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _ = SetProperty(ref _selectedStatus, value);
                RaisePropertyChanged(nameof(SelectedStatusForeground));
                RaisePropertyChanged(nameof(SelectedStatusBackground));
            }
        }

        /// <summary>
        /// 選択された区分の前景色
        /// </summary>
        public Color SelectedCategoryForeground
        {
            get => SelectedCategory == null || string.IsNullOrEmpty(SelectedCategory.ForegroundColorHex)
                ? (Color)ColorConverter.ConvertFromString("#00000000")
                : (Color)ColorConverter.ConvertFromString(SelectedCategory.ForegroundColorHex);
            set
            {
                SelectedCategory.ForegroundColorHex = $"#{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}";
                File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

        /// <summary>
        /// 選択された区分の背景色
        /// </summary>
        public Color SelectedCategoryBackground
        {
            get => SelectedCategory == null || string.IsNullOrEmpty(SelectedCategory.BackgroundColorHex)
                ? (Color)ColorConverter.ConvertFromString("#FFFFFFFF")
                : (Color)ColorConverter.ConvertFromString(SelectedCategory.BackgroundColorHex);
            set
            {
                SelectedCategory.BackgroundColorHex = $"#{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}";
                File.WriteAllText(_categoriesFilePath, JsonSerializer.Serialize(Categories));
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

        /// <summary>
        /// 選択された状況の前景色
        /// </summary>
        public Color SelectedStatusForeground
        {
            get => SelectedStatus == null || string.IsNullOrEmpty(SelectedStatus.ForegroundColorHex)
                ? (Color)ColorConverter.ConvertFromString("#00000000")
                : (Color)ColorConverter.ConvertFromString(SelectedStatus.ForegroundColorHex);
            set
            {
                SelectedStatus.ForegroundColorHex = $"#{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}";
                File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
                RaisePropertyChanged(nameof(SelectedStatus));
            }
        }

        /// <summary>
        /// 選択された状況の背景色
        /// </summary>
        public Color SelectedStatusBackground
        {
            get => SelectedStatus == null || string.IsNullOrEmpty(SelectedStatus.BackgroundColorHex)
                    ? (Color)ColorConverter.ConvertFromString("#FFFFFFFF")
                    : (Color)ColorConverter.ConvertFromString(SelectedStatus.BackgroundColorHex);
            set
            {
                SelectedStatus.BackgroundColorHex = $"#{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}";
                File.WriteAllText(_statusesFilePath, JsonSerializer.Serialize(Statuses));
                RaisePropertyChanged(nameof(SelectedStatus));
            }
        }
        #endregion

        #region IDialogAware
        /// <summary>
        /// ダイアログのタイトル
        /// </summary>
        public string Title => "Configure Style";

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

        }

        /// <summary>
        /// ダイアログが開いたときのイベントハンドラ
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Categories = parameters.GetValue<List<ToDoCategory>>("Categories");
            Statuses = parameters.GetValue<List<ToDoStatus>>("Statuses");
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 区分一覧のファイルパス
        /// </summary>
        private static readonly string _categoriesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\categories.json";
        /// <summary>
        /// 状況一覧のファイルパス
        /// </summary>
        private static readonly string _statusesFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\statuses.json";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigureStyleDialogViewModel()
        {

        }
        #endregion
    }
}
