#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Todo.Shared.Controls
{
    public partial class NativeProgressBar : Control
    {
        private UIElement CreateNativeView()
        {
            throw new NotImplementedException();
        }
    }
}
#endif