using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDragDrop.Classes
{
    public class Account : INotifyPropertyChanged
    {
        public Guid Id { get; private set; }

        private decimal balance;

        public decimal Balance
        {
            get { return balance; }
            set
            {
                if (balance != value)
                {
                    balance = value;
                    RaiseProperty(nameof(Balance));
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaiseProperty(nameof(Name));
                }
            }
        }
        private string name;

        public Account(string name, decimal balance)
        {
            this.name = name;
            this.balance = balance;
            Id = Guid.NewGuid();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperty(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
