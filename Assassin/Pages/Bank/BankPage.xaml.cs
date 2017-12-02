using Assassin.Classes;
using Extensions;
using System.ComponentModel;
using System.Windows;

namespace Assassin.Pages.Bank
{
    /// <summary>Interaction logic for BankPage.xaml</summary>
    public partial class BankPage : INotifyPropertyChanged
    {
        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        #region Display Manipulation

        /// <summary>Adds text to the TxtBank Textbox.</summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTt(string newText) => Functions.AddTextToTextBox(TxtBank, newText);

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
            await GameState.SaveUser(GameState.CurrentUser);
        }

        public BankPage() => InitializeComponent();

        private void BankPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            GameState.CalculateScale(Grid);
            DataContext = GameState.CurrentUser;
            CheckButtons();
        }

        #endregion Page-Manipulation Methods
    }
}