using MinatoProject.Apps.ToDoCoreWpf.Core.Native;
using MinatoProject.Apps.ToDoCoreWpf.Core.Services;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels
{
    /// <summary>
    /// ApplicationPreferenceDialog.xamlのViewModelクラス
    /// </summary>
    public class ApplicationPreferenceDialogViewModel : BindableBase, IDialogAware
    {
        #region コマンド
        /// <summary>
        /// キーフック設定フォーカス取得コマンド
        /// </summary>
        public DelegateCommand OnGotFocusCommand { get; private set; }
        /// <summary>
        /// キーフック設定フォーカス喪失コマンド
        /// </summary>
        public DelegateCommand OnLostFocusCommand { get; private set; }
        #endregion

        #region プロパティ
        /// <summary>
        /// 通知領域格納フラグ
        /// </summary>
        public bool ExitAsMinimized
        {
            get => _settings != null && _settings.GetSettings().ExitAsMinimized;
            set
            {
                _settings?.SetExitAsMinimized(value);
                RaisePropertyChanged(nameof(ExitAsMinimized));
            }
        }

        private string _hookKeysText = string.Empty;
        /// <summary>
        /// フックするキーの文字列表現
        /// </summary>
        public string HookKeysText
        {
            get => _hookKeysText;
            set => _ = SetProperty(ref _hookKeysText, value);
        }
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
            KeyboardHook.RemoveEvent(HookKeyboard);
            KeyboardHook.Resume();
        }

        /// <summary>
        /// ダイアログが開いたときのイベントハンドラ
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 設定情報
        /// </summary>
        private readonly SettingsStore _settings;
        /// <summary>
        /// 押下されているキーのリスト
        /// </summary>
        private readonly List<Keys> _detectedKeys = new();
        /// <summary>
        /// 元の設定値
        /// </summary>
        private readonly List<Keys> _currentKeys = new();
        /// <summary>
        /// フックがキャンセルされたか
        /// </summary>
        private bool _isCanceled;
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationPreferenceDialogViewModel()
        {
            _logger.Info("start");
            _settings = SettingsStore.GetInstance();
            if (_settings.GetSettings().HookKeys.Count > 0)
            {
                _currentKeys = new List<Keys>(_settings.GetSettings().HookKeys);
                HookKeysText = string.Join(" + ", _currentKeys);
            }

            // コマンドの登録
            OnGotFocusCommand = new DelegateCommand(ExecuteOnGotFocusCommand);
            OnLostFocusCommand = new DelegateCommand(ExecuteOnLostFocusCommand);
            _logger.Info("end");
        }
        #endregion

        /// <summary>
        /// キーフック設定フォーカス取得コマンドを実行する
        /// </summary>
        private void ExecuteOnGotFocusCommand()
        {
            _logger.Info("start");
            _detectedKeys.Clear();
            HookKeysText = string.Empty;

            KeyboardHook.AddEvent(HookKeyboard);
            KeyboardHook.Resume();
            _logger.Info("end");
        }

        /// <summary>
        /// キーフック設定フォーカス喪失コマンドを実行する
        /// </summary>
        private void ExecuteOnLostFocusCommand()
        {
            _logger.Info("start");
            KeyboardHook.RemoveEvent(HookKeyboard);
            KeyboardHook.Pause();

            if (!_isCanceled)
            {
                _settings.SetHookKeys(_detectedKeys);
            }
            _isCanceled = false;
            _logger.Info("end");
        }

        /// <summary>
        /// キーフックイベント
        /// </summary>
        /// <param name="s"></param>
        private void HookKeyboard(ref KeyboardHook.StateKeyboard s)
        {
            switch (s.Stroke)
            {
                case KeyboardHook.Stroke.KEY_DOWN:
                case KeyboardHook.Stroke.SYSKEY_DOWN:
                    if (s.Key == Keys.Escape)
                    {
                        _detectedKeys.Clear();
                        HookKeysText = string.Join(" + ", _currentKeys);
                        Keyboard.ClearFocus();
                        KeyboardHook.RemoveEvent(HookKeyboard);
                        KeyboardHook.Pause();
                        _isCanceled = true;
                    }
                    else
                    {
                        if (!_detectedKeys.Contains(s.Key))
                        {
                            _detectedKeys.Add(s.Key);
                            HookKeysText = string.Join(" + ", _detectedKeys);
                        }
                    }
                    break;

                case KeyboardHook.Stroke.KEY_UP:
                case KeyboardHook.Stroke.SYSKEY_UP:
                    if (!_detectedKeys.Contains(s.Key))
                    {
                        _ = _detectedKeys.Remove(s.Key);
                        HookKeysText = string.Join(" + ", _detectedKeys);
                    }
                    break;

                case KeyboardHook.Stroke.UNKNOWN:
                default:
                    break;
            }
        }
    }
}
