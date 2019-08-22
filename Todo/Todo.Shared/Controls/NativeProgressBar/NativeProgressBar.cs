using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Todo.Shared.Controls
{
    public partial class NativeProgressBar : Control
    {

#if __ANDROID__
        private Android.Views.View _nativeView;
#elif __IOS__
        private UIKit.UIView _nativeView;
#else
        private Windows.UI.Xaml.UIElement _nativeView;
#endif

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _nativeView = CreateNativeView();
        }
    }
}
