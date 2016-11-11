using System;
using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window, INotifyPropertyChanged
    {
        internal MainWindow RefToMainWindow { get; set; }
        private string nl = Environment.NewLine;

        internal void NewUser()
        {
            txtGame.Text = "Creare An Vita, " + GameState.CurrentUser.Name + "!" + nl + nl + "You enter the city of thieves to take your place among the legends!";
        }

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        /// <summary>
        /// Adds text to the txtGame Textbox.
        /// </summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTT(string newText)
        {
            txtGame.Text += nl + nl + newText;
            txtGame.Focus();
            txtGame.CaretIndex = txtGame.Text.Length;
            txtGame.ScrollToEnd();
        }

        #region Button-Click Methods

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnPub_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnInn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnJail_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnGuild_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnOthers_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnMystic_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnChapel_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnRob_Click(object sender, RoutedEventArgs e)
        {
            //BattleWindow battleWindow = new BattleWindow();
            //battleWindow.RefToGameWindow = this;
            //battleWindow.Show();
            //this.Visibility = Visibility.Hidden;
        }

        private void btnAssassinate_Click(object sender, RoutedEventArgs e)
        {
            AssassinationWindow assassinationWindow = new AssassinationWindow();
            assassinationWindow.RefToGameWindow = this;
            assassinationWindow.GetEnemy();
            assassinationWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnShops_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnTrain_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnBank_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        private void CloseWindow()
        {
            this.Close();
        }

        public GameWindow()
        {
            InitializeComponent();
            DataContext = GameState.CurrentUser;
        }

        private async void windowGame_Closing(object sender, CancelEventArgs e)
        {
            await GameState.SaveUser(GameState.CurrentUser);
            RefToMainWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}