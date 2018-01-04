using DPWithItemCollection.Classes;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DPWithItemCollection
{
    public sealed partial class TestControl : UserControl
    {
        public Test MyData
        {
            get { return (Test)GetValue(MyDataProperty); }
            set { SetValue(MyDataProperty, value); }
        }

        public static readonly DependencyProperty MyDataProperty =
            DependencyProperty.Register("MyData", typeof(Test), typeof(TestControl), new PropertyMetadata(null));

        public TestControl()
        {
            this.InitializeComponent();
        }
    }
}
