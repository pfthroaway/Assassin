using Assassin.Models;
using Assassin.Views.Admin;
using Assassin.Views.Help;
using System.Windows;

namespace Assassin.Views
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        #region Menu Click Methods

        private void MnuFileExit_Click(object sender, RoutedEventArgs e) => Close();

        private void MnuAdmin_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new AdminLoginPage());

        private void MnuHelpManual_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ManualPage());

        private void MnuHelpAbout_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new AboutPage());

        #endregion Menu Click Methods

        #region Window-Manipulation Methods

        public MainWindow() => InitializeComponent();

        private async void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            GameState.MainWindow = this;
            new SplashWindow(this).Show();
            await GameState.LoadAll();
        }

        #endregion Window-Manipulation Methods
    }
}