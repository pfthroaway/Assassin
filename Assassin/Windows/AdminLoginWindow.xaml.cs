using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {
        internal MainWindow RefToMainWindow { get; set; }
        private bool blnAdmin = false;

        #region Button-Click Methods

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordHash.ValidatePassword(pswdAdmin.Password.Trim(), GameState.AdminPassword))
            {
                blnAdmin = true;
                CloseWindow();
            }
            else
                MessageBox.Show("Invalid login.", "Assassin", MessageBoxButton.OK);
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
            pswdAdmin.SelectAll();
        }

        private void pswdAdmin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pswdAdmin.Password.Length > 0)
                btnSubmit.IsEnabled = true;
            else
                btnSubmit.IsEnabled = false;
        }

        private void windowAdminLogin_Closing(object sender, CancelEventArgs e)
        {
            if (!blnAdmin)
                RefToMainWindow.Show();
            else
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.RefToMainWindow = RefToMainWindow;
                adminWindow.Show();
            }
        }

        #endregion Window-Manipulation
    }
}