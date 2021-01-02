using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Create_ResX_File
{
    public sealed partial class PrivacyPolice : SettingsFlyout
    {
        public PrivacyPolice()
        {
            this.InitializeComponent();
        }

        private async void TextBlock_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var uri = new Uri("mailto:xtrose@hotmail.de");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
