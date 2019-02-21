using Assassin.Classes;
using Extensions;
using Extensions.DataTypeHelpers;
using Extensions.Enums;
using System.Windows;
using System.Windows.Input;

namespace Assassin.Pages.Bank
{
    /// <summary>Interaction logic for BankDialogPage.xaml</summary>
    public partial class BankDialogPage
    {
        private int _maximum, _textAmount;
        private string _type = "";

        internal BankPage RefToBankPage { private get; set; }

        /// <summary>Load the necessary data for the Page.</summary>
        /// <param name="maximum">Maximum amount of gold to be used.</param>
        /// <param name="type">What type of transaction is taking place.</param>
        internal void LoadPage(int maximum, string type)
        {
            _maximum = maximum;
            _type = type;

            switch (_type)
            {
                case "Deposit":
                    LblDialog.Text = $"How much gold would you like to deposit? You have {_maximum:N0} gold with you.";
                    BtnAction.Content = "_Deposit";
                    break;

                case "Withdrawal":
                    LblDialog.Text =
                        $"How much gold would you like to withdraw? You have {_maximum:N0} gold in your account.";
                    BtnAction.Content = "_Withdraw";
                    break;

                case "Repay Loan":
                    LblDialog.Text =
                        $"How much of your loan would you like to repay? You currently owe {_maximum:N0} gold. You have {GameState.CurrentUser.GoldOnHand:N0} with you.";
                    BtnAction.Content = "_Repay";
                    break;

                case "Take Out Loan":
                    LblDialog.Text =
                        $"How much gold would you like to take out on loan? Your credit deems you worthy of receiving up to {maximum:N0} gold. Remember, we have a 5% loan fee.";
                    BtnAction.Content = "_Borrow";
                    break;
            }
        }

        #region Transaction Methods

        /// <summary>Deposit money into the bank.</summary>
        private void Deposit()
        {
            GameState.CurrentUser.GoldInBank += _textAmount;
            GameState.CurrentUser.GoldOnHand -= _textAmount;
            ClosePage($"You deposit {_textAmount:N0} gold.");
        }

        /// <summary>Repay the loan.</summary>
        private void RepayLoan()
        {
            GameState.CurrentUser.GoldOnLoan -= _textAmount;
            GameState.CurrentUser.GoldOnHand -= _textAmount;
            ClosePage($"You repay {_textAmount:N0} gold on your loan.");
        }

        /// <summary>Take out a loan.</summary>
        private void TakeOutLoan()
        {
            GameState.CurrentUser.GoldOnLoan += _textAmount + _textAmount / 20;
            GameState.CurrentUser.GoldOnHand += _textAmount;
            ClosePage($"You take out a loan for {_textAmount:N0} gold.");
        }

        /// <summary>Withdraw money from the bank account.</summary>
        private void Withdrawal()
        {
            GameState.CurrentUser.GoldInBank -= _textAmount;
            GameState.CurrentUser.GoldOnHand += _textAmount;
            ClosePage($"You withdraw {_textAmount:N0} gold from your account.");
        }

        #endregion Transaction Methods

        #region Button-Click Methods

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            _textAmount = Int32Helper.Parse(TxtBank.Text);

            if (_textAmount <= _maximum && _textAmount > 0)
            {
                switch (_type)
                {
                    case "Deposit":
                        if (_textAmount <= GameState.CurrentUser.GoldOnHand)
                            Deposit();
                        else
                            GameState.DisplayNotification(
                                $"Please enter a value less than or equal to your current gold. You currently have {GameState.CurrentUser.GoldOnHandToString} gold.", "Assassin");
                        break;

                    case "Withdrawal":
                        Withdrawal();
                        break;

                    case "Repay Loan":
                        if (_textAmount <= GameState.CurrentUser.GoldOnHand)
                            RepayLoan();
                        else
                            GameState.DisplayNotification(
                                $"Please enter a value less than or equal to your current gold. You currently have {GameState.CurrentUser.GoldOnHandToString} gold.", "Assassin");
                        break;

                    case "Take Out Loan":
                        TakeOutLoan();
                        break;
                }
            }
            else
                GameState.DisplayNotification(
                    $"Please enter a positive value less than or equal to your current gold. You currently have {GameState.CurrentUser.GoldOnHandToString} gold.", "Assassin");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage("");

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page</summary>
        /// <param name="text">Text to be passed back to the Bank Page</param>
        private void ClosePage(string text)
        {
            if (text.Length > 0)
                RefToBankPage.AddTextTt(text);
            RefToBankPage.CheckButtons();
            GameState.GoBack();
        }

        public BankDialogPage()
        {
            InitializeComponent();
            TxtBank.Focus();
        }

        private void TxtBank_GotFocus(object sender, RoutedEventArgs e) => Functions.TextBoxGotFocus(sender);

        private void TxtBank_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Integers);

        #endregion Page-Manipulation Methods
    }
}