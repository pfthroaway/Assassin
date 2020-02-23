using Assassin.Models;
using Extensions;
using System.Windows;

namespace Assassin.Views.Shopping
{
    /// <summary>Interaction logic for ShopsPage.xaml</summary>
    public partial class ShopsPage
    {
        #region Button-Click Methods

        private void BtnAlchemistsLab_Click(object sender, RoutedEventArgs e)
        {
            // TODO Implement the Alchemist's Lab - Potions.
        }

        private void BtnArmory_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ArmoryPage());

        private void BtnGeneralStore_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new GeneralStorePage());

        private void BtnMagicalRealms_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new MagicalRealmsPage());

        private void BtnWeapons_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new WeaponsRUsPage());

        private void BtnThieves_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ThievesGuildPage());

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Functions.AddTextToTextBox(GameState.GamePage.TxtGame, TxtShops.Text.Trim());
            GameState.GoBack();
            GameState.ShopsPage = null;
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public ShopsPage()
        {
            InitializeComponent();
            TxtShops.Text = "You enter the shopping district.";
            GameState.ShopsPage = this;
        }

        #endregion Page-Manipulation Methods
    }
}