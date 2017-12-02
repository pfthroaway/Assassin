using Assassin.Classes;
using Assassin.Classes.Enums;
using System.Windows;

namespace Assassin.Pages.Player
{
    /// <summary>Interaction logic for InventoryPage.xaml</summary>
    public partial class InventoryPage
    {
        /// <summary>Checks which Buttons should be enabled.</summary>
        private void CheckButtons()
        {
            BtnEquipLight.IsEnabled = GameState.CurrentUser.CurrentWeapon != WeaponType.Light;
            BtnEquipHeavy.IsEnabled = GameState.CurrentUser.CurrentWeapon != WeaponType.Heavy;
            BtnEquipTwoHanded.IsEnabled = GameState.CurrentUser.CurrentWeapon != WeaponType.TwoHanded;
            BtnDrinkPotion.IsEnabled = GameState.CurrentUser.Potion.Name != "None";
        }

        /// <summary>Equips a <see cref="Classes.Items.Weapon"/></summary>
        /// <param name="type">Weapon type to equip.</param>
        private void EquipWeapon(WeaponType type)
        {
            GameState.CurrentUser.CurrentWeapon = type;
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
            await GameState.SaveUser(GameState.CurrentUser);
        }

        public InventoryPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GameState.CalculateScale(Grid);
            DataContext = GameState.CurrentUser;
            CheckButtons();
        }

        #endregion Page-Manipulation Methods
    }
}