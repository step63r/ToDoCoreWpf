using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    /// <summary>
    /// ApplicationPreferenceDialog.xamlのViewModelクラス
    /// </summary>
    public class ApplicationPreferenceDialogViewModel : BindableBase, IDialogAware
    {
        #region コマンド
        /// <summary>
        /// OKコマンド
        /// </summary>
        public DelegateCommand OkCommand { get; private set; }
        #endregion

        #region IDialogAware
        /// <summary>
        /// ダイアログのタイトル
        /// </summary>
        public string Title => "Preferences";

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
            // コマンドの登録
            OkCommand = new DelegateCommand(ExecuteOkCommand);
        }

        /// <summary>
        /// ダイアログが開いたときのイベントハンドラ
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationPreferenceDialogViewModel()
        {

        }
        #endregion

        /// <summary>
        /// OKコマンドを実行する
        /// </summary>
        private void ExecuteOkCommand()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
    }
}
