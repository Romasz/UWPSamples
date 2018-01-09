using AccountDragDrop.Classes;
using AccountDragDrop.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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

namespace AccountDragDrop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string accountId = "ACCOUNT";

        public ObservableCollection<Account> Accounts;

        public MainPage()
        {
            this.InitializeComponent();
            Accounts = new ObservableCollection<Account>
            {
                new Account("First", 10000),
                new Account("Second", -10000),
                new Account("Third", 50000)
            };
        }

        private void AccountControl_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Link;
            e.DragUIOverride.Caption = "Transfer";
        }

        private void AccountControl_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            if ((sender as AccountControl)?.DataContext is Account account)
            {
                args.AllowedOperations = DataPackageOperation.Link;
                args.Data.SetData(accountId, account.Id);
            }
        }

        private async void AccountControl_Drop(object sender, DragEventArgs e)
        {
            if ((e.OriginalSource as AccountControl)?.DataContext is Account targetAccount)
                if (await (e.DataView.GetDataAsync(accountId)) is Guid sourceAccountId)
                {
                    var sourceAccount = Accounts.First(x => x.Id == sourceAccountId);
                    sourceAccount.Balance -= 1000;
                    targetAccount.Balance += 1000;
                }
        }
    }
}
