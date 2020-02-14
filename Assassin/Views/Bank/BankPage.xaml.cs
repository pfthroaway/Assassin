using Assassin.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Assassin.Views.Bank
{
    /// <summary>Interaction logic for BankPage.xaml</summary>
    public partial class BankPage : INotifyPropertyChanged
    {
        #region Data-Binding

        /// <summary>Event that executes if a Property value has changed so that the UI can properly be updated.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyNames">The names of the properties to update in the UI.</param>
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyName">The optional name of the property to update in the UI. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Data-Binding

        #region Display Manipulation

        /// <summary>Checks what buttons should be enabled.</summary>
        internal void CheckButtons()
        {
            BtnDeposit.IsEnabled = GameState.CurrentUser.GoldOnHand > 0;
            BtnWithdraw.IsEnabled = GameState.CurrentUser.GoldInBank > 0;
            BtnTakeLoan.IsEnabled = GameState.CurrentUser.LoanAvailable > 0;
            BtnRepayLoan.IsEnabled = GameState.CurrentUser.GoldOnLoan > 0;
        }

        /// <summary> Displays the Bank Dialog Page. </summary>
        /// <param name="maximum">Maximum amount of gold permitted</param>
        /// <param name="type">Type of Page information to be displayed</param>
        private void DisplayBankDialog(int maximum, string type)
        {
            BankDialogPage bankDialogPage = new BankDialogPage();
            bankDialogPage.LoadPage(maximum, type);
            bankDialogPage.RefToBankPage = this;
            GameState.Navigate(bankDialogPage);
        }

        #endregion Display Manipulation

        #region Button-Click Methods

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnDeposit_Click(object sender, RoutedEventArgs e) => DisplayBankDialog(GameState.CurrentUser.GoldOnHand, "Deposit");

        private void BtnRepayLoan_Click(object sender, RoutedEventArgs e) => DisplayBankDialog(GameState.CurrentUser.GoldOnLoan, "Repay Loan");

        private void BtnTakeLoan_Click(object sender, RoutedEventArgs e) => DisplayBankDialog(GameState.CurrentUser.LoanAvailable, "Take Out Loan");

        private void BtnWithdraw_Click(object sender, RoutedEventArgs e) => DisplayBankDialog(GameState.CurrentUser.GoldInBank, "Withdrawal");

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private async void ClosePage()
        {
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        public BankPage()
        {
            InitializeComponent();
            TxtBank.Text = "You enter the bank. The teller greets you.\n\n" +
                "Welcome to The Bank, the only secure place to store gold in the city!";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = GameState.CurrentUser;
            CheckButtons();
        }

        #endregion Page-Manipulation Methods
    }
}