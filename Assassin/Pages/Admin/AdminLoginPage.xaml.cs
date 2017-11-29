using Assassin.Classes;
using Extensions;
using Extensions.Encryption;
using System.Windows;

namespace Assassin.Pages.Admin
{
    /// <summary>Interaction logic for AdminLoginPage.xaml</summary>
    public partial class AdminLoginPage
    {
        private bool _blnAdmin;

        #region Button-Click Methods

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdAdmin.Password.Trim(), GameState.AdminPassword))
            {
                _blnAdmin = true;
                ClosePage();
            }
            else
                GameState.DisplayNotification("Invalid login.", "Assassin");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation

        /// <summary>Closes the Page.</summary>
        private void ClosePage()
        {
            GameState.GoBack();
            if (_blnAdmin)
                GameState.Navigate(new AdminPage());
            else
                GameState.MainWindow.MnuAdmin.IsEnabled = true;
        }

        public AdminLoginPage()
        {
            InitializeComponent();
            PswdAdmin.Focus();
        }

        private void AdminLoginPage_OnLoaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);

        private void pswdAdmin_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void pswdAdmin_PasswordChanged(object sender, RoutedEventArgs e) => BtnSubmit.IsEnabled =
            PswdAdmin.Password.Length > 0;

        #endregion Page-Manipulation
    }
}