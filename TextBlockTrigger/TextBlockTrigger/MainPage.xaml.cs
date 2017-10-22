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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TextBlockTrigger
{
    public class EqualParamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => value.ToString() == parameter.ToString();
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }

    public sealed partial class MainPage : Page
    {
        public MainPage() { this.InitializeComponent(); }

        private void ToggleButton_Click(object sender, RoutedEventArgs e) => myTextBlck.Text = (bool)(sender as ToggleButton).IsChecked ? "On" : "Off";
    }
}
