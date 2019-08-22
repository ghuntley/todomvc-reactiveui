#if __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Todo.Shared.Controls
{
    public partial class NativeProgressBar : Control
    {
        private Windows.UI.Xaml.UIElement CreateNativeView()
        {
            throw new NotImplementedException();
		}
    }
}
#endif