
namespace AsteriskTCGMaker4.Views.Behaviors
{
    using System;
    using System.Windows;
    internal class CommonDialogBehavior
    {
        #region Callback 添付プロパティ
        /// <summary>
        /// Action&lt;bool, string&gt; 型の Callback 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty CallbackProperty =
       DependencyProperty.RegisterAttached("Callback", typeof(Action<bool, string>),
       typeof(CommonDialogBehavior), new PropertyMetadata(null));
        /// <summary>
        /// Callback 添付プロパティを取得します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <returns>取得した値を返します。</returns>
        public static Action<bool, string> GetCallback(DependencyObject target)
        {
            return (Action<bool, string>)target.GetValue(CallbackProperty);
        }
        /// <summary>
        /// Callback 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetCallback(DependencyObject target, Action<bool, string> value)
        {
            target.SetValue(CallbackProperty, value);
        }
        #endregion Callback 添付プロパティ
    }
}

