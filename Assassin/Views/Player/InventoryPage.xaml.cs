using Assassin.Models;
using Assassin.Models.Enums;
using Assassin.Models.Items;
using System.Windows;

namespace Assassin.Views.Player
{
    /// <summary>Interaction logic for InventoryPage.xaml</summary>
    public partial class InventoryPage
    {
        /// <summary>Checks which Buttons should be enabled.</summary>
        private void CheckButtons()
        {
            BtnEquipLight.IsEnabled = GameState.CurrentUser.CurrentWeaponType != WeaponType.Light;
            BtnEquipHeavy.IsEnabled = GameState.CurrentUser.CurrentWeaponType != WeaponType.Heavy;
            BtnEquipTwoHanded.IsEnabled = GameState.CurrentUser.CurrentWeaponType != WeaponType.TwoHanded;
            BtnDrinkPotion.IsEnabled = GameState.CurrentUser.Potion.Name != "None";
        }

        /// <summary>Equips a <see cref="Weapon"/></summary>
        /// <param name="type"><see cref="WeaponType"/> to equip.</param>
        private void EquipWeapon(WeaponType type)
        {
            GameState.CurrentUser.CurrentWeaponType = type;
            CheckButtons();
        }

        #region Button-Click Methods

        private void BtnEquipLight_Click(object sender, RoutedEventArgs e) => EquipWeapon(WeaponType.Light);

        private void BtnEquipHeavy_Click(object sender, RoutedEventArgs e) => EquipWeapon(WeaponType.Heavy);

        private void BtnEquipTwoHanded_Click(object sender, RoutedEventArgs e) => EquipWeapon(WeaponType.TwoHanded);

        private void BtnDrinkPotion_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Heal(GameState.CurrentUser.Potion.HealAmount);
            GameState.CurrentUser.Potion = GameState.AllPotions.Find(potion => potion.Name == "None");
            CheckButtons();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        private async void ClosePage()
        {
            GameState.GoBack();
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        public InventoryPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = GameState.CurrentUser;
            CheckButtons();
        }

        #endregion Page-Manipulation Methods
    }
}