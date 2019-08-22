#if __ANDROID__
using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Todo.Shared.Controls
{
    public partial class NativeProgressBar : Control
    {
        private View CreateNativeView()
        {
            // nb. TODO: Context must be set.
            return new Android.Widget.ProgressBar(null);
        }
    }
}
#endif
