using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Login

        internal void Login()
        {
            txtUsername.Text = "";
            pswdPassword.Password = "";
            txtUsername.Focus();
            GameWindow gameWindow = new GameWindow();
            gameWindow.RefToMainWindow = this;
            gameWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        #endregion Login

        #region Click Methods

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow newUserWindow = new NewUserWindow();
            newUserWindow.RefToMainWindow = this;
            newUserWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.CheckLogin(txtUsername.Text.Trim(), pswdPassword.Password.Trim()))
            {
                Login();
            }
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminLoginWindow adminLoginWindow = new AdminLoginWindow();
            adminLoginWindow.RefToMainWindow = this;
            adminLoginWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void mnuHelpManual_Click(object sender, RoutedEventArgs e)
        {
            ManualWindow manualWindow = new ManualWindow();
            manualWindow.RefToMainWindow = this;
            manualWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void mnuHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.RefToMainWindow = this;
            aboutWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

        private void TextChanged()
        {
            if (txtUsername.Text.Length >= 4 && pswdPassword.Password.Length >= 4)
                btnLogin.IsEnabled = true;
            else
                btnLogin.IsEnabled = false;
        }

        public MainWindow()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

        private async void windowMain_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(() => GameState.LoadAll());
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUsername.SelectAll();
        }

        private void txtUsername_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key k = e.Key;

            List<bool> keys = GameState.GetListOfKeys(Key.Back, Key.Delete, Key.Home, Key.End, Key.Enter, Key.Tab, Key.Left, Key.Right, Key.Escape, Key.LeftShift, Key.RightShift, Key.LeftAlt, Key.RightAlt, Key.LeftCtrl, Key.RightCtrl);

            if (keys.Any(key => key == true) || Key.A <= k && k <= Key.Z)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtUsername.Text = new string((from c in txtUsername.Text
                                           where char.IsLetter(c)
                                           select c).ToArray());
            txtUsername.CaretIndex = txtUsername.Text.Length;
            TextChanged();
        }

        private void pswdPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            TextChanged();
        }

        private void pswdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pswdPassword.SelectAll();
        }

        private void windowMain_Closing(object sender, CancelEventArgs e)
        {
        }

        #endregion Window-Manipulation Methods
    }
}