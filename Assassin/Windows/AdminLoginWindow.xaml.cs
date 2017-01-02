using Extensions;
using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow
    {
        internal MainWindow RefToMainWindow { get; set; }
        private bool blnAdmin;

        #region Button-Click Methods

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordHash.ValidatePassword(pswdAdmin.Password.Trim(), GameState.AdminPassword))
            {
                blnAdmin = true;
                CloseWindow();
            }
            else
                new Notification("Invalid login.", "Assassin", NotificationButtons.OK, this).ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation

        private void CloseWindow()
        {
            this.Close();
        }

        public AdminLoginWindow()
        {
            InitializeComponent();
            pswdAdmin.Focus();
        }

        private void pswdAdmin_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void pswdAdmin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            btnSubmit.IsEnabled = pswdAdmin.Password.Length > 0;
        }

        private void windowAdminLogin_Closing(object sender, CancelEventArgs e)
        {
            if (!blnAdmin)
                RefToMainWindow.Show();
            else
            {
                AdminWindow adminWindow = new AdminWindow { RefToMainWindow = RefToMainWindow };
                adminWindow.Show();
            }
        }

        #endregion Window-Manipulation
    }
}