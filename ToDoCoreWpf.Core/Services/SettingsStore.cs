using MinatoProject.Apps.ToDoCoreWpf.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace MinatoProject.Apps.ToDoCoreWpf.Core.Services
{
    /// <summary>
    /// 設定情報をストアするクラス
    /// </summary>
    public sealed class SettingsStore : ServiceStoreBase
    {
        #region Singleton
        /// <summary>
        /// シングルトン インスタンス
        /// </summary>
        private static readonly SettingsStore _instance = new();

        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public static SettingsStore GetInstance()
        {
            return _instance;
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 設定情報
        /// </summary>
        private Settings _settings = new();
        /// <summary>
        /// 設定情報ファイルパス
        /// </summary>
        private static readonly string _settingsFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\ToDoCoreWpf\settings.json";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingsStore()
        {
        }
        #endregion

        #region メソッド
        /// <summary>
        /// インスタンスを初期化する
        /// </summary>
        public override void InitializeInstance()
        {
            // ファイルが存在しない場合、新規作成
            if (!File.Exists(_settingsFilePath))
            {
                File.WriteAllText(_settingsFilePath, JsonSerializer.Serialize(_settings));
            }
            _settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(_settingsFilePath));
        }

        /// <summary>
        /// 設定情報を取得する
        /// </summary>
        /// <returns>設定情報</returns>
        public Settings GetSettings()
        {
            return _settings;
        }

        /// <summary>
        /// 通知領域格納フラグを設定する
        /// </summary>
        /// <param name="value">設定値</param>
        public void SetExitAsMinimized(bool value)
        {
            _settings.ExitAsMinimized = value;
            File.WriteAllText(_settingsFilePath, JsonSerializer.Serialize(_settings));
        }

        /// <summary>
        /// フックするキー
        /// </summary>
        /// <param name="value"></param>
        public void SetHookKeys(List<Keys> value)
        {
            _settings.HookKeys = value;
            File.WriteAllText(_settingsFilePath, JsonSerializer.Serialize(_settings));
        }
        #endregion
    }
}
