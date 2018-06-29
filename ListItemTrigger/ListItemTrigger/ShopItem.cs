using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListItemTrigger
{
    public class ShopItem : INotifyPropertyChanged
    {
        public bool IsBookAvailable
        {
            get { return isBookAvailable; }
            set {
                if (isBookAvailable != value)
                {
                    isBookAvailable = value;
                    Notify(nameof(IsBookAvailable));
                }
            }
        }
        private bool isBookAvailable = false;

        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
