using System.Windows;
using System.Windows.Input;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Behaviours
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://qiita.com/namitan/items/53f3500ed471352195a8</remarks>
    internal static class KeyboardFocusBehaviour
    {
        #region 依存関係プロパティ
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty OnProperty;
        #endregion

        #region getter/setter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static FrameworkElement GetOn(UIElement element)
        {
            return (FrameworkElement)element.GetValue(OnProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetOn(UIElement element, FrameworkElement value)
        {
            element.SetValue(OnProperty, value);
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        static KeyboardFocusBehaviour()
        {
            OnProperty = DependencyProperty.RegisterAttached("On", typeof(FrameworkElement), typeof(KeyboardFocusBehaviour), new PropertyMetadata(OnSetCallback));
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSetCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                var target = GetOn(frameworkElement);

                if (target == null)
                {
                    return;
                }

                frameworkElement.Loaded += (s, e) => Keyboard.Focus(target);
            }
        }
    }
}
