using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWithItemCollection.Classes
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> MyList { get; set; }

        public Test()
        {
            Name = "Unknown";
            MyList = new ObservableCollection<string>();
        }
    }
}
