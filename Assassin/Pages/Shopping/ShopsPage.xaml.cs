using Assassin.Classes;
using System.Windows;

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for ShopsPage.xaml</summary>
    public partial class ShopsPage
    {
        #region Button-Click Methods

        private void BtnArmory_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ArmoryPage());

        private void BtnGeneralStore_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new GeneralStorePage());

        private void BtnWeapons_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new WeaponsRUsPage());

        private void BtnThieves_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ThievesGuildPage());

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public ShopsPage()
        {
            InitializeComponent();
            TxtShops.Text = "You enter the shopping district.";
        }

        #endregion Page-Manipulation Methods
    }
}