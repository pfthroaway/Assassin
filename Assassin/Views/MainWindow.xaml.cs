using Assassin.Models;
using Assassin.Views.Admin;
using Assassin.Views.Help;
using System.ComponentModel;
using System.Windows;

namespace Assassin.Views
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        /// <summary>Prevent the Window from closing if during battle or court.</summary>
        public bool BlnPreventClosing { get; set; }

        /// <summary>Text to be displayed if the Window is being prevented from closing.</summary>
        public string TxtPreventClosing { get; set; }

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

        private void WindowMain_Closing(object sender, CancelEventArgs e)
        {
            if (BlnPreventClosing)
            {
                e.Cancel = true;
                GameState.DisplayNotification(TxtPreventClosing, "Assassin");
            }

            #endregion Window-Manipulation Methods
        }
    }
}