using System;
using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for BankWindow.xaml
    /// </summary>
    public partial class BankWindow :  INotifyPropertyChanged
    {
        private readonly string nl = Environment.NewLine;

        internal GameWindow RefToGameWindow { get; set; }

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Display Manipulation

        /// <summary>
        /// Adds text to the txtBank Textbox.
        /// </summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTT(string newText)
        {
            txtBank.Text += nl + nl + newText;
            txtBank.Focus();
            txtBank.CaretIndex = txtBank.Text.Length;
            txtBank.ScrollToEnd();
        }

        /// <summary>
        /// Checks what buttons should be enabled.
        /// </summary>
        internal void CheckButtons()
        {
            btnDeposit.IsEnabled = GameState.CurrentUser.GoldOnHand > 0;
            btnWithdraw.IsEnabled = GameState.CurrentUser.GoldInBank > 0;
            btnTakeLoan.IsEnabled = GameState.CurrentUser.LoanAvailable > 0;
            btnRepayLoan.IsEnabled = GameState.CurrentUser.GoldOnLoan > 0;
        }

        /// <summary>
        /// Displays the Bank Dialog Window.
        /// </summary>
        /// <param name="maximum">Maximum amount of gold permitted</param>
        /// <param name="type">Type of Window information to be displayed</param>
        private void DisplayBankDialog(int maximum, string type)
        {
            BankDialogWindow bankDialogWindow = new BankDialogWindow();
            bankDialogWindow.LoadWindow(maximum, type);
            bankDialogWindow.RefToBankWindow = this;
            bankDialogWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        #endregion Display Manipulation

        #region Button-Click Methods

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void btnDeposit_Click(object sender, RoutedEventArgs e)
        {
            DisplayBankDialog(GameState.CurrentUser.GoldOnHand, "Deposit");
        }

        private void btnRepayLoan_Click(object sender, RoutedEventArgs e)
        {
            DisplayBankDialog(GameState.CurrentUser.GoldOnLoan, "Repay Loan");
        }

        private void btnTakeLoan_Click(object sender, RoutedEventArgs e)
        {
            DisplayBankDialog(GameState.CurrentUser.LoanAvailable, "Take Out Loan");
        }

        private void btnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            DisplayBankDialog(GameState.CurrentUser.GoldInBank, "Withdrawal");
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>
        /// Closes the Window.
        /// </summary>
        private void CloseWindow()
        {
            this.Close();
        }

        public BankWindow()
        {
            InitializeComponent();
            DataContext = GameState.CurrentUser;
            CheckButtons();
        }

        private async void windowBank_Closing(object sender, CancelEventArgs e)
        {
            await GameState.SaveUser(GameState.CurrentUser);
            RefToGameWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}