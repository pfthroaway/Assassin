using Assassin.Classes;
using Assassin.Pages.Admin;
using Assassin.Pages.Help;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Assassin.Pages
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        #region Menu Click Methods

        private void MnuFileExit_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

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