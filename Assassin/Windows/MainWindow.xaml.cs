using Extensions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Login

        internal void Login()
        {
            txtUsername.Text = "";
            pswdPassword.Password = "";
            txtUsername.Focus();
            GameWindow gameWindow = new GameWindow { RefToMainWindow = this };
            gameWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        #endregion Login

        #region Click Methods

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow newUserWindow = new NewUserWindow { RefToMainWindow = this };
            newUserWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.CheckLogin(txtUsername.Text.Trim(), pswdPassword.Password.Trim()))
                Login();
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminLoginWindow adminLoginWindow = new AdminLoginWindow { RefToMainWindow = this };
            adminLoginWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void mnuHelpManual_Click(object sender, RoutedEventArgs e)
        {
            ManualWindow manualWindow = new ManualWindow { RefToMainWindow = this };
            manualWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void mnuHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow { RefToMainWindow = this };
            aboutWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

        private void TextChanged()
        {
            btnLogin.IsEnabled = txtUsername.Text.Length >= 1 && pswdPassword.Password.Length >= 1;
        }

        public MainWindow()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

        private async void windowMain_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(GameState.LoadAll);
            new SplashWindow(this).ShowDialog();
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.TextBoxGotFocus(sender);
        }

        private void txtUsername_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Functions.PreviewKeyDown(e, KeyType.Letters);
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            Functions.TextBoxTextChanged(sender, KeyType.Letters);
            TextChanged();
        }

        private void pswdPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            TextChanged();
        }

        private void pswdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        #endregion Window-Manipulation Methods
    }
}