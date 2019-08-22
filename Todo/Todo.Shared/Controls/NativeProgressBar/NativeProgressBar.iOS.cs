#if __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Windows.UI.Xaml.Controls;

namespace Todo.Shared.Controls
{
    public partial class NativeProgressBar : Control
    {
        private UIView CreateNativeView()
        {
            return new UIProgressView();
		}
    }
}
#endif