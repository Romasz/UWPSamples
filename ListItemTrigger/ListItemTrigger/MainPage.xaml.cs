using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ListItemTrigger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ShopItem> ShopList { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            ShopList = new ObservableCollection<ShopItem>
            {
                new ShopItem{ Title = "First", IsBookAvailable = false},
                new ShopItem{ Title = "Second", IsBookAvailable = false},
                new ShopItem{ Title = "Third", IsBookAvailable = true},
                new ShopItem{ Title = "Fourth", IsBookAvailable = false}
            };

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var shopItem = (e.OriginalSource as FrameworkElement).DataContext as ShopItem;
            shopItem.IsBookAvailable = !shopItem.IsBookAvailable;
        }
    }
}
