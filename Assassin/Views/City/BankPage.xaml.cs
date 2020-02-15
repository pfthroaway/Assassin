using Assassin.Models;
using Extensions;
using Extensions.DataTypeHelpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Assassin.Views.City
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

        #endregion Display Manipulation

        /// <summary>Displays an InputDialog and attempts to get a value.</summary>
        /// <param name="text">Text to be displayed in the dialog</param>
        /// <returns>Value from the InputDialog</returns>
        private int DisplayDialog(string text, int maximum)
        {
            string dialogAmount = GameState.InputDialog(text, "Assassin");
            if (dialogAmount.Length > 0)
            {
                int amount = Int32Helper.Parse(dialogAmount);
                if (amount > 0 && amount <= maximum)
                    return amount;
                else
                    GameState.DisplayNotification($"Please enter a positive integer value above 0 and below {maximum:N0}.", "Assassin");
            }
            return 0;
        }

        #region Button-Click Methods

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnDeposit_Click(object sender, RoutedEventArgs e)
        {
            int amount = DisplayDialog($"How much gold would you like to deposit? You have {GameState.CurrentUser.GoldOnHandToString} gold with you.", GameState.CurrentUser.GoldOnHand);
            if (amount > 0)
            {
                GameState.CurrentUser.GoldInBank += amount;
                GameState.CurrentUser.GoldOnHand -= amount;
                Functions.AddTextToTextBox(TxtBank, $"You deposit {amount:N0} gold.");
                CheckButtons();
            }
        }

        private void BtnRepayLoan_Click(object sender, RoutedEventArgs e)
        {
            int amount = DisplayDialog($"How much of your loan would you like to repay? You currently owe {GameState.CurrentUser.GoldOnLoanToString} gold. You have {GameState.CurrentUser.GoldOnHandToString} with you.", GameState.CurrentUser.GoldOnLoan);
            if (amount > 0)
            {
                GameState.CurrentUser.GoldOnLoan -= amount;
                Functions.AddTextToTextBox(TxtBank, $"You repay {amount:N0} gold on your loan.");
                CheckButtons();
            }
        }

        private void BtnTakeLoan_Click(object sender, RoutedEventArgs e)
        {
            int amount = DisplayDialog($"How much gold would you like to take out on loan? Your credit deems you worthy of receiving up to {GameState.CurrentUser.LoanAvailableToString} gold. Remember, we have a 5% loan fee.", GameState.CurrentUser.LoanAvailable);
            if (amount > 0)
            {
                GameState.CurrentUser.GoldOnLoan += amount + (amount / 20);
                GameState.CurrentUser.GoldOnHand += amount;
                Functions.AddTextToTextBox(TxtBank, $"You take out a loan for {amount:N0} gold.");
                CheckButtons();
            }
        }

        private void BtnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            int amount = DisplayDialog($"How much gold would you like to withdraw? You have {GameState.CurrentUser.GoldInBankToString} gold in your account.", GameState.CurrentUser.GoldInBank);
            if (amount > 0)
            {
                GameState.CurrentUser.GoldOnHand += amount;
                GameState.CurrentUser.GoldInBank -= amount;
                Functions.AddTextToTextBox(TxtBank, $"You withdraw {amount:N0} gold from your account.");
                CheckButtons();
            }
        }

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