using Assassin.Classes;
using Assassin.Pages.City;
using Assassin.Pages.Player;
using Extensions;
using Extensions.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin.Pages
{
    /// <summary>Interaction logic for LoginPage.xaml</summary>
    public partial class LoginPage
    {
        #region Login

        internal void Login()
        {
            TxtUsername.Text = "";
            PswdPassword.Password = "";
            TxtUsername.Focus();
            GameState.Navigate(new GamePage());
        }

        #endregion Login

        #region Click Methods

        private void BtnNewUser_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new NewUserPage());

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.CheckLogin(TxtUsername.Text.Trim(), PswdPassword.Password.Trim()))
                Login();
        }

        #endregion Click Methods

        #region Page-Manipulation Methods

        private void TextChanged() => BtnLogin.IsEnabled = TxtUsername.Text.Length >= 1 && PswdPassword.Password.Length >= 1;

        public LoginPage()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void TxtUsername_GotFocus(object sender, RoutedEventArgs e) => Functions.TextBoxGotFocus(sender);

        private void TxtUsername_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Letters);

        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            Functions.TextBoxTextChanged(sender, KeyType.Letters);
            TextChanged();
        }

        private void pswdPassword_TextChanged(object sender, RoutedEventArgs e) => TextChanged();

        private void pswdPassword_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void Page_Loaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);

        #endregion Page-Manipulation Methods
    }
}