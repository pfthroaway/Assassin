using Assassin.Classes;
using Extensions;
using Extensions.Encryption;
using System.Windows;

namespace Assassin.Pages.Options
{
    /// <summary>Interaction logic for ChangePasswordPage.xaml</summary>
    public partial class ChangePasswordPage
    {
        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, GameState.CurrentUser.Password))
                if (PswdNewPassword.Password.Length >= 4 && PswdConfirmPassword.Password.Length >= 4)
                    if (PswdNewPassword.Password == PswdConfirmPassword.Password)
                        if (PswdCurrentPassword.Password != PswdNewPassword.Password)
                        {
                            GameState.CurrentUser.Password = PBKDF2.HashPassword(PswdNewPassword.Password);
                            await GameState.ChangeUserDetails(GameState.CurrentUser, GameState.CurrentUser);
                            GameState.DisplayNotification("Successfully changed password.", "Assassin");
                            ClosePage();
                        }
                        else
                            GameState.DisplayNotification("The new password can't be the same as the current password.", "Assassin");
                    else
                        GameState.DisplayNotification("Please ensure the new passwords match.", "Assassin");
                else
                    GameState.DisplayNotification("Your password must be at least 4 characters.", "Assassin");
            else
                GameState.DisplayNotification("Invalid current password.", "Assassin");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => GameState.GoBack();

        public ChangePasswordPage()
        {
            InitializeComponent();
            PswdCurrentPassword.Focus();
        }

        private void PswdChanged(object sender, RoutedEventArgs e) => BtnSubmit.IsEnabled = PswdCurrentPassword.Password.Length > 0 && PswdNewPassword.Password.Length > 0 && PswdConfirmPassword.Password.Length > 0;

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void HeroChangePasswordPage_OnLoaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);

        private void Page_Loaded(object sender, RoutedEventArgs e) => GameState.CalculateScale(Grid);

        #endregion Page-Manipulation Methods
    }
}