using System;
using System.Runtime.InteropServices;

namespace MinatoProject.Apps.ToDoCoreWpf.Native
{
    /// <summary>
    /// 
    /// </summary>
    internal class InterceptKeyboard : AbstractInterceptKeyboard
    {
        #region InputEvent
        /// <summary>
        /// 
        /// </summary>
        public class OriginalKeyEventArg : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public int KeyCode { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="keyCode">キーコード</param>
            public OriginalKeyEventArg(int keyCode) => KeyCode = keyCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void KeyEventHandler(object sender, OriginalKeyEventArg e);
        /// <summary>
        /// 
        /// </summary>
        public event KeyEventHandler KeyDownEvent;
        /// <summary>
        /// 
        /// </summary>
        public event KeyEventHandler KeyUpEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCode"></param>
        protected void OnKeyDownEvent(int keyCode)
        {
            KeyDownEvent?.Invoke(this, new OriginalKeyEventArg(keyCode));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCode"></param>
        protected void OnKeyUpEvent(int keyCode)
        {
            KeyUpEvent?.Invoke(this, new OriginalKeyEventArg(keyCode));
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                int vkCode = (int)kb.vkCode;
                OnKeyDownEvent(vkCode);
            }
            else if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                int vkCode = (int)kb.vkCode;
                OnKeyUpEvent(vkCode);
            }

            return base.HookProcedure(nCode, wParam, lParam);
        }
    }
}
