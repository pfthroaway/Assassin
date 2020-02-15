using Assassin.Models;
using Assassin.Views.Options;
using System.Windows;

namespace Assassin.Views.Admin
{
    /// <summary>Interaction logic for AdminPage.xaml</summary>
    public partial class AdminPage
    {
        #region Button-Click Methods

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ChangePasswordPage(true));

        private void BtnManageEnemies_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnManageGuilds_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnManageUsers_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private static void ClosePage()
        {
            GameState.MainWindow.MainFrame.RemoveBackEntry();
            GameState.GoBack();
            GameState.MainWindow.MnuAdmin.IsEnabled = true;
        }

        public AdminPage() => InitializeComponent();

        #endregion Page-Manipulation Methods
    }
}