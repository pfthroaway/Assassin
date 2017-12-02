using Assassin.Classes;
using Assassin.Pages.Bank;
using Assassin.Pages.Battle;
using Assassin.Pages.Player;
using Assassin.Pages.Shopping;
using Extensions;
using System.ComponentModel;
using System.Windows;

namespace Assassin.Pages.City
{
    /// <summary>Interaction logic for GamePage.xaml</summary>
    public partial class GamePage : INotifyPropertyChanged
    {
        internal void NewUser() => TxtGame.Text =
                $"Creare An Vita, {GameState.CurrentUser.Name}!\n\nYou enter the city of thieves to take your place among the legends!";

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        /// <summary>Adds text to the TxtGame Textbox.</summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTt(string newText) => Functions.AddTextToTextBox(TxtGame, newText);

        #region Button-Click Methods

        private void BtnExit_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnMessages_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnOptions_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnPub_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnInn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnJail_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnGuild_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnOthers_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnMystic_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnChapel_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnRob_Click(object sender, RoutedEventArgs e)
        {
            //BattlePage battlePage = new BattlePage();
            //battlePage.RefToGamePage = this;
            //battlePage.Show();
            //this.Visibility = Visibility.Hidden;
        }

        private void BtnAssassinate_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new AssassinationPage());

        private void BtnInventory_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new InventoryPage());

        private void BtnShops_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ShopsPage());

        private void BtnTrain_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new TrainPage());

        private void BtnBank_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new BankPage());

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        private async void ClosePage()
        {
            GameState.GoBack();
            await GameState.SaveUser(GameState.CurrentUser);
        }

        public GamePage() => InitializeComponent();

        private void GamePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            GameState.CalculateScale(Grid);
            DataContext = GameState.CurrentUser;
        }

        #endregion Page-Manipulation Methods
    }
}