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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DPWithItemCollection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Test FirstTest { get; set; }
        public Test SecondTest { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            FirstTest = new Test { Name = "First list:" };
            SecondTest = new Test { Name = "Second list:" };
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            FirstTest.MyList.Add(DateTime.Now.ToString("hh:mm:ss"));
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            SecondTest.MyList.Add(DateTime.Now.ToString("hh:mm:ss"));
        }
    }
}
