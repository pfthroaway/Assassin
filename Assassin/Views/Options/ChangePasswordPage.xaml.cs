using Assassin.Models;
using Extensions;
using Extensions.Encryption;
using System.Windows;

namespace Assassin.Views.Options
{
    /// <summary>Interaction logic for ChangePasswordPage.xaml</summary>
    public partial class ChangePasswordPage
    {
        private bool _blnAdmin;

        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PswdNewPassword.Password.Length >= 4 && PswdConfirmPassword.Password.Length >= 4)
            {
                if (PswdNewPassword.Password == PswdConfirmPassword.Password)
                {
                    if (PswdCurrentPassword.Password != PswdNewPassword.Password)
                    {
                        if (!_blnAdmin)
                        {
                            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, GameState.CurrentUser.Password))
                            {
                                GameState.CurrentUser.Password = PBKDF2.HashPassword(PswdNewPassword.Password);
                                if (await GameState.ChangeUserDetails(GameState.CurrentUser, GameState.CurrentUser))
                                {
                                    GameState.DisplayNotification("Successfully changed password.", "Assassin");
                                    ClosePage();
                                }
                            }
                            else
                                GameState.DisplayNotification("Invalid current password.", "Assassin");
                        }
                        else
                        {
                            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, GameState.AdminPassword))
                            {
                                GameState.AdminPassword = PBKDF2.HashPassword(PswdNewPassword.Password);
                                if (await GameState.DatabaseInteraction.ChangeAdminPassword(GameState.AdminPassword))
                                {
                                    GameState.DisplayNotification("Successfully changed admin password.", "Assassin");
                                    ClosePage();
                                }
                            }
                            else
                                GameState.DisplayNotification("Invalid current password.", "Assassin");
                        }
                    }
                    else
                        GameState.DisplayNotification("The new password can't be the same as the current password.", "Assassin");
                }
                else
                    GameState.DisplayNotification("Please ensure the new passwords match.", "Assassin");
            }
            else
                GameState.DisplayNotification("Your password must be at least 4 characters.", "Assassin");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => GameState.GoBack();

        public ChangePasswordPage(bool admin = false)
        {
            InitializeComponent();
            _blnAdmin = admin;
            PswdCurrentPassword.Focus();
        }

        private void PswdChanged(object sender, RoutedEventArgs e) => BtnSubmit.IsEnabled = PswdCurrentPassword.Password.Length > 0 && PswdNewPassword.Password.Length > 0 && PswdConfirmPassword.Password.Length > 0;

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        #endregion Page-Manipulation Methods
    }
}