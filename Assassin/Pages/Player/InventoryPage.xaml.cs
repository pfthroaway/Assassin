using Assassin.Classes;
using Assassin.Classes.Enums;
using System.ComponentModel;
using System.Windows;

namespace Assassin.Pages.Player
{
    /// <summary>Interaction logic for InventoryPage.xaml</summary>
    public partial class InventoryPage : INotifyPropertyChanged
    {
        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        #region Button-Click Methods

        private void BtnEquipLight_Click(object sender, RoutedEventArgs e) => GameState.CurrentUser.CurrentWeapon = WeaponType.Light;

        private void BtnEquipHeavy_Click(object sender, RoutedEventArgs e) => GameState.CurrentUser.CurrentWeapon = WeaponType.Heavy;

        private void BtnEquipTwoHanded_Click(object sender, RoutedEventArgs e) => GameState.CurrentUser.CurrentWeapon = WeaponType.TwoHanded;

        private void BtnDrinkPotion_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Heal(GameState.CurrentUser.Potion.HealAmount);
            GameState.CurrentUser.Potion = GameState.AllPotions.Find(potion => potion.Name == "None");
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        private async void ClosePage()
        {
            GameState.GoBack();
            await GameState.SaveUser(GameState.CurrentUser);
        }

        public InventoryPage() => InitializeComponent();

        #endregion Page-Manipulation Methods
    }
}