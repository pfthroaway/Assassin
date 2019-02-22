using Assassin.Classes;
using Assassin.Classes.Entities;
using Assassin.Classes.Enums;
using Assassin.Classes.Items;
using Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Assassin.Pages.Shopping
{
    /// <summary>Interaction logic for WeaponsRUsPage.xaml</summary>
    public partial class WeaponsRUsPage : INotifyPropertyChanged
    {
        private Weapon _selectedWeapon = new Weapon();
        private Weapon _currentWeapon = new Weapon();
        private List<Weapon> _weaponsList = new List<Weapon>();

        #region Properties

        /// <summary>The currently selected <see cref="Weapon"/>.</summary>
        public Weapon SelectedWeapon
        {
            get => _selectedWeapon;
            set
            {
                _selectedWeapon = value;
                OnPropertyChanged("SelectedWeapon");
                GrpSelected.DataContext = SelectedWeapon;
            }
        }

        /// <summary>The <see cref="Weapon"/> the <see cref="User"/> currently has equipped of the selected type.</summary>
        public Weapon CurrentWeapon
        {
            get => _currentWeapon; set
            {
                _currentWeapon = value;
                OnPropertyChanged("CurrentWeapon");
                GrpCurrent.DataContext = CurrentWeapon;
            }
        }

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        /// <summary>Updates the Page's binding.</summary>
        private void UpdateBinding()
        {
            DataContext = GameState.CurrentUser;
            LstWeapon.ItemsSource = _weaponsList;
        }

        #endregion Data-Binding

        #region Purchase/Sell

        /// <summary>Checks whether the currently selected <see cref="Weapon"/> can be purchased.</summary>
        private void CheckPurchase() => BtnPurchase.IsEnabled = SelectedWeapon != new Weapon() && GameState.CurrentUser.GoldOnHand >= SelectedWeapon.Value && SelectedWeapon != CurrentWeapon;

        /// <summary>Checks whether the currently equipped <see cref="Weapon"/> can be sold.</summary>
        private void CheckSell() => BtnSell.IsEnabled = CurrentWeapon.SellValue > 0;

        /// <summary>Asks if the current <see cref="User"/>'s wants to sell their <see cref="Weapon"/>.</summary>
        private bool AskSell() => GameState.YesNoNotification($"Art thou sure thou want to sell thy {CurrentWeapon.Name} for { CurrentWeapon.SellValueToString} gold?", "Assassin");

        /// <summary>Sells the current <see cref="User"/>'s <see cref="Weapon"/>.</summary>
        private void Sell()
        {
            Functions.AddTextToTextBox(TxtWeapon, $"Thou hast sold thy {CurrentWeapon.Name} for {CurrentWeapon.SellValueToString} gold.");
            GameState.CurrentUser.GoldOnHand += CurrentWeapon.SellValue;
            switch (CurrentWeapon.Type)
            {
                case WeaponType.Light:
                    GameState.CurrentUser.LightWeapon = GameState.AllWeapons.Find(wpn => wpn.Name == "Hands" && wpn.Type == WeaponType.Light);
                    CurrentWeapon = GameState.CurrentUser.LightWeapon;
                    break;

                case WeaponType.Heavy:
                    GameState.CurrentUser.HeavyWeapon = GameState.AllWeapons.Find(wpn => wpn.Name == "Hands" && wpn.Type == WeaponType.Heavy);
                    CurrentWeapon = GameState.CurrentUser.HeavyWeapon;
                    break;

                case WeaponType.TwoHanded:
                    GameState.CurrentUser.TwoHandedWeapon = GameState.AllWeapons.Find(wpn => wpn.Name == "Hands" && wpn.Type == WeaponType.TwoHanded);
                    CurrentWeapon = GameState.CurrentUser.TwoHandedWeapon;
                    break;
            }

            CheckSell();
            UpdateBinding();
        }

        /// <summary>Purchases the selected <see cref="Weapon"/>.</summary>
        private void Purchase()
        {
            Functions.AddTextToTextBox(TxtWeapon, $"Thou hast purchased the {SelectedWeapon.Name} for {SelectedWeapon.ValueToString} gold.");
            GameState.CurrentUser.GoldOnHand -= SelectedWeapon.Value;

            switch (SelectedWeapon.Type)
            {
                case WeaponType.Light:
                    GameState.CurrentUser.LightWeapon = SelectedWeapon;
                    CurrentWeapon = GameState.CurrentUser.LightWeapon;
                    break;

                case WeaponType.Heavy:
                    GameState.CurrentUser.HeavyWeapon = SelectedWeapon;
                    CurrentWeapon = GameState.CurrentUser.HeavyWeapon;
                    break;

                case WeaponType.TwoHanded:
                    GameState.CurrentUser.TwoHandedWeapon = SelectedWeapon;
                    CurrentWeapon = GameState.CurrentUser.TwoHandedWeapon;
                    break;
            }

            CheckSell();
            CheckPurchase();
            UpdateBinding();
        }

        #endregion Purchase/Sell

        #region Checked-Changed

        /// <summary>Checks which <see cref="Weapon"/> type to be displayed for purchase when a radio button's checked status changes.</summary>
        /// <param name="type"><see cref="WeaponType"/> to determine which <see cref="Weapon"/> type to display.</param>
        private void CheckedChanged(WeaponType type)
        {
            _weaponsList = GameState.AllWeapons.FindAll(wpn => wpn.Type == type && !wpn.Hidden);
            LstWeapon.SelectedIndex = 0;
            switch (type)
            {
                case (WeaponType.Light):
                    CurrentWeapon = GameState.CurrentUser.LightWeapon;
                    break;

                case (WeaponType.Heavy):
                    CurrentWeapon = GameState.CurrentUser.HeavyWeapon;
                    break;

                case (WeaponType.TwoHanded):
                    CurrentWeapon = GameState.CurrentUser.TwoHandedWeapon;
                    break;
            }

            UpdateBinding();
        }

        private void RadLight_Checked(object sender, RoutedEventArgs e) => CheckedChanged(WeaponType.Light);

        private void RadHeavy_Checked(object sender, RoutedEventArgs e) => CheckedChanged(WeaponType.Heavy);

        private void RadTwoH_Checked(object sender, RoutedEventArgs e) => CheckedChanged(WeaponType.TwoHanded);

        #endregion Checked-Changed

        #region Click-Methods

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentWeapon.SellValue > 0)
            {
                if (AskSell())
                {
                    Sell();
                    Purchase();
                }
            }
            else
                Purchase();
        }

        private void BtnSell_Click(object sender, RoutedEventArgs e)
        {
            if (AskSell())
                Sell();
        }

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            GameState.GoBack();
            await GameState.SaveUser(GameState.CurrentUser);
        }

        private void LstWeapon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstWeapon.SelectedIndex < 0)
                LstWeapon.SelectedIndex = 0;
            SelectedWeapon = (Weapon)LstWeapon.SelectedItem;
            UpdateBinding();
            CheckPurchase();
        }

        #endregion Click-Methods

        #region Page-Manipulation Methods

        public WeaponsRUsPage()
        {
            InitializeComponent();
            TxtWeapon.Text = "Thou hast entered Weapons 'R Us. The weaponsmith greets you.\n\n" +
                "Weapons 'R Us is the name, tools of destruction is our game.";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RadLight.IsChecked = true;
            UpdateBinding();
            CheckSell();
        }

        #endregion Page-Manipulation Methods
    }
}