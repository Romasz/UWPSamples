using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmptyListTrigger
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperty(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ObservableCollection<string> Tasks { get; set; } = new ObservableCollection<string>() { "Starting item" };
        public bool IsTasksEmpty => Tasks.Count < 1;

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            Tasks.CollectionChanged += (sender, e) => RaiseProperty(nameof(IsTasksEmpty));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) => Tasks.Add("Next item");
        private void DelButton_Click(object sender, RoutedEventArgs e) => Tasks.RemoveAt(Tasks.Count - 1);
    }
}
