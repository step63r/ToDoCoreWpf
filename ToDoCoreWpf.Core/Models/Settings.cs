using System.Collections.Generic;
using System.Windows.Forms;

namespace MinatoProject.Apps.ToDoCoreWpf.Core.Models
{
    /// <summary>
    /// 設定情報
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// 通知領域格納フラグ
        /// </summary>
        public bool ExitAsMinimized { get; set; }

        /// <summary>
        /// フックするキー
        /// </summary>
        public List<Keys> HookKeys { get; set; } = new List<Keys>() { Keys.RShiftKey };
    }
}
