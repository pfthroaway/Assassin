using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow :  INotifyPropertyChanged
    {
        internal GameWindow RefToGameWindow { get; set; }
        internal BattleWindow RefToBattleWindow { get; set; }
        private string previousWindow = "";

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        internal void SetPreviousWindow(string windowName)
        {
            previousWindow = windowName;
        }

        #region Button-Click Methods

        private void btnEquipLight_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.CurrentWeapon = WeaponType.Light;
        }

        private void btnEquipHeavy_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.CurrentWeapon = WeaponType.Heavy;
        }

        private void btnEquipTwoHanded_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.CurrentWeapon = WeaponType.TwoHanded;
        }

        private void btnDrinkPotion_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Heal(GameState.CurrentUser.Potion.HealAmount);
            GameState.CurrentUser.Potion = GameState.AllPotions.Find(potion => potion.Name == "None");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        private void CloseWindow()
        {
            this.Close();
        }

        public InventoryWindow()
        {
            InitializeComponent();
        }

        private async void windowInventory_Closing(object sender, CancelEventArgs e)
        {
            await GameState.SaveUser(GameState.CurrentUser);
            switch (previousWindow)
            {
                case "Battle":
                    RefToBattleWindow.Show();
                    break;

                case "Game":
                    RefToGameWindow.Show();
                    break;
            }
        }

        #endregion Window-Manipulation Methods
    }
}