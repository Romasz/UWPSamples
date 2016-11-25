using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TextBlockWithLinks
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperty(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string textToFormat;
        public string TextToFormat
        {
            get { return textToFormat; }
            set { textToFormat = value; RaiseProperty(nameof(TextToFormat)); }
        }


        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Show_Click(object sender, RoutedEventArgs e)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///sample.json"));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                TextToFormat = JObject.Parse(await sRead.ReadToEndAsync()).GetValue("post_clean").ToString();
        }
    }
}
