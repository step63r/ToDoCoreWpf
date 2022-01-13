using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Behaviours
{
    /// <summary>
    /// TextBoxで日本語IME使用時に変換中BackSpaceのキャレット移動がおかしいのを修正するビヘイビア
    /// </summary>
    /// <remarks>https://social.msdn.microsoft.com/Forums/pt-BR/aa037e96-b59d-4b79-9477-929e013bd98e/wpf12398textbox20869123952608526412354862083721147123771242726178?forum=wpfja</remarks>
    internal class TextBoxImeFixBehaviour
    {
        #region メンバ変数
        /// <summary>
        /// キャレット位置補正有無
        /// </summary>
        private static bool _imeStart = false;
        /// <summary>
        /// 補正対象となっているか
        /// </summary>
        private static bool _fixTarget = false;
        #endregion

        #region 依存関係プロパティ
        /// <summary>
        /// キャレット位置補正有無の依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ImeFixProperty =
            DependencyProperty.RegisterAttached(
                "ImeFix", typeof(bool), typeof(TextBoxImeFixBehaviour),
                new UIPropertyMetadata(default(bool), new PropertyChangedCallback(OnImeFixPropertyChanged)));
        #endregion

        #region getter / setter
        /// <summary>
        /// キャレット位置補正有無を取得する
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetImeFix(DependencyObject obj)
        {
            return (bool)obj.GetValue(ImeFixProperty);
        }

        /// <summary>
        /// キャレット位置補正有無を設定する
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetImeFix(DependencyObject obj, bool value)
        {
            obj.SetValue(ImeFixProperty, value);
        }
        #endregion

        /// <summary>
        /// キャレット位置補正有無変更時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnImeFixPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                bool newValue = (bool)e.NewValue;
                bool oldValue = (bool)e.OldValue;

                if (oldValue)
                {
                    textBox.TextChanged -= TextBox_TextChanged;
                    TextCompositionManager.RemovePreviewTextInputHandler(textBox, TextBox_PreviewTextInput);
                    TextCompositionManager.RemoveTextInputUpdateHandler(textBox, TextBox_TextInputUpdate);
                }
                if (newValue)
                {
                    textBox.TextChanged += TextBox_TextChanged;
                    TextCompositionManager.AddPreviewTextInputHandler(textBox, TextBox_PreviewTextInput);
                    TextCompositionManager.AddTextInputUpdateHandler(textBox, TextBox_TextInputUpdate);
                }
            }
        }

        #region イベントハンドラ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (_fixTarget)
            {
                textBox.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    int i = textBox.CaretIndex;
                    textBox.CaretBrush = null;
                    textBox.CaretIndex = i;
                }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_imeStart)
            {
                _imeStart = false;
                _fixTarget = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TextBox_TextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            if (_imeStart)
            {
                if (_fixTarget)
                {
                    textBox.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }
            }
            else
            {
                int i = textBox.CaretIndex;
                if (i > 0 && textBox.Text[i - 1] == '\n')
                {
                    _fixTarget = true;
                }
                _imeStart = true;
            }
        }
        #endregion
    }
}
