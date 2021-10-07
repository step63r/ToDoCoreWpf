using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MinatoProject.Apps.ToDoCoreWpf.Native
{
    /// <summary>
    /// キーボードフックの抽象クラス
    /// </summary>
    internal abstract class AbstractInterceptKeyboard
    {
        #region Win32 Constants
        /// <summary>
        /// フックの種類（ローレベルフック）
        /// </summary>
        protected const int WH_KEYBOARD_LL = 0x000D;
        /// <summary>
        /// 
        /// </summary>
        protected const int WM_KEYDOWN = 0x0100;
        /// <summary>
        /// 
        /// </summary>
        protected const int WM_KEYUP = 0x0101;
        /// <summary>
        /// 
        /// </summary>
        protected const int WM_SYSKEYDOWN = 0x0104;
        /// <summary>
        /// 
        /// </summary>
        protected const int WM_SYSKEYUP = 0x0105;
        #endregion

        #region Win32API Structures
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            KEYEVENTF_SCANCODE = 0x0008,
            KEYEVENTF_UNICODE = 0x0004,
        }
        #endregion

        #region Win32 Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hMod"></param>
        /// <param name="dwThreadId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hhk"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hhk"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion

        #region Delegate
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private KeyboardProc proc;
        /// <summary>
        /// 
        /// </summary>
        private IntPtr hookId = IntPtr.Zero;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Hook()
        {
            if (hookId == IntPtr.Zero)
            {
                proc = HookProcedure;
                using var curProcess = Process.GetCurrentProcess();
                using var curModule = curProcess.MainModule;
                hookId = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnHook()
        {
            _ = UnhookWindowsHookEx(hookId);
            hookId = IntPtr.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public virtual IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }
    }
}
