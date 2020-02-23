using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Views.City;
using Extensions;
using Extensions.Encryption;
using Extensions.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin.Views.Player
{
    /// <summary>Interaction logic for NewUserPage.xaml</summary>
    public partial class NewUserPage : INotifyPropertyChanged
    {
        private bool _blnStart;
        private User _createUser = new User();
        private readonly User _defaultUser = new User();

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyNames">The names of the properties to update in the UI.</param>
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>Notifys the PropertyChanged event alerting the WPF Framework to update the UI.</summary>
        /// <param name="propertyName">The optional name of the property to update in the UI. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Data-Binding

        #region Button-Manipulation Methods

        /// <summary>Check which buttons to enable.</summary>
        private void CheckButtons()
        {
            if (_createUser.SkillPoints > 0)
            {
                TogglePlusButtons(true);
                BtnCreate.IsEnabled = false;

                if (_createUser.SkillPoints == _defaultUser.SkillPoints)
                {
                    DisableMinusButtons();
                    BtnReset.IsEnabled = false;
                }
                else
                    BtnReset.IsEnabled = true;
            }
            else
            {
                TogglePlusButtons(false);
                BtnReset.IsEnabled = true;

                BtnCreate.IsEnabled = TxtUsername.Text.Trim().Length >= 1 && PswdConfirm.Password.Length >= 1 && PswdPassword.Password.Length >= 1;
            }
        }

        /// <summary>Disable Minus buttons.</summary>
        private void DisableMinusButtons()

        {
            BtnEnduranceMinus.IsEnabled = false;
            BtnLightWeaponsMinus.IsEnabled = false;
            BtnHeavyWeaponsMinus.IsEnabled = false;
            BtnTwoHandedWeaponsMinus.IsEnabled = false;
            BtnBlockingMinus.IsEnabled = false;
            BtnSlippingMinus.IsEnabled = false;
            BtnStealthMinus.IsEnabled = false;
        }

        /// <summary>Toggles the Plus buttons.</summary>
        /// <param name="enabled">Should the Plus buttons be enabled?</param>
        private void TogglePlusButtons(bool enabled)
        {
            BtnEndurancePlus.IsEnabled = enabled;
            BtnLightWeaponsPlus.IsEnabled = enabled;
            BtnHeavyWeaponsPlus.IsEnabled = enabled;
            BtnTwoHandedWeaponsPlus.IsEnabled = enabled;
            BtnBlockingPlus.IsEnabled = enabled;
            BtnSlippingPlus.IsEnabled = enabled;
            BtnStealthPlus.IsEnabled = enabled;
        }

        #endregion Button-Manipulation Methods

        #region Attribute Modification

        /// <summary>Increases the passed attribute.</summary>
        /// <param name="attribute">Attribute to be increased</param>
        /// <returns>Increased attribute</returns>
        private int IncreaseAttribute(int attribute)
        {
            attribute += 8;
            _createUser.SkillPoints--;
            CheckButtons();
            return attribute;
        }

        /// <summary>Decreases the passed attribute.</summary>
        /// <param name="attribute">Attribute to be decreased</param>
        /// <returns>Decreased attribute</returns>
        private int DecreaseAttribute(int attribute)
        {
            attribute -= 8;
            _createUser.SkillPoints++;
            CheckButtons();
            return attribute;
        }

        #endregion Attribute Modification

        #region Plus/Minus Buttons

        private void BtnEnduranceMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.SkillPoints++;
            _createUser.CurrentEndurance -= 20;
            _createUser.MaximumEndurance -= 20;
            BtnEnduranceMinus.IsEnabled = _createUser.MaximumEndurance != _defaultUser.MaximumEndurance;
            CheckButtons();
        }

        private void BtnEndurancePlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.SkillPoints--;
            _createUser.CurrentEndurance += 20;
            _createUser.MaximumEndurance += 20;
            BtnEnduranceMinus.IsEnabled = true;
            CheckButtons();
        }

        private void BtnLightWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.LightWeaponSkill = DecreaseAttribute(_createUser.LightWeaponSkill);
            BtnLightWeaponsMinus.IsEnabled = GameState.CurrentUser.LightWeaponSkill != _defaultUser.LightWeaponSkill;
        }

        private void BtnLightWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.LightWeaponSkill = IncreaseAttribute(_createUser.LightWeaponSkill);
            BtnLightWeaponsMinus.IsEnabled = true;
        }

        private void BtnHeavyWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.HeavyWeaponSkill = DecreaseAttribute(_createUser.HeavyWeaponSkill);
            BtnHeavyWeaponsMinus.IsEnabled = GameState.CurrentUser.HeavyWeaponSkill != _defaultUser.HeavyWeaponSkill;
        }

        private void BtnHeavyWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.HeavyWeaponSkill = IncreaseAttribute(_createUser.HeavyWeaponSkill);
            BtnHeavyWeaponsMinus.IsEnabled = true;
        }

        private void BtnTwoHandedWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.TwoHandedWeaponSkill = DecreaseAttribute(_createUser.TwoHandedWeaponSkill);
            BtnTwoHandedWeaponsMinus.IsEnabled = GameState.CurrentUser.TwoHandedWeaponSkill != _defaultUser.TwoHandedWeaponSkill;
        }

        private void BtnTwoHandedWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.TwoHandedWeaponSkill = IncreaseAttribute(_createUser.TwoHandedWeaponSkill);
            BtnTwoHandedWeaponsMinus.IsEnabled = true;
        }

        private void BtnBlockingMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Blocking = DecreaseAttribute(_createUser.Blocking);
            BtnSlippingMinus.IsEnabled = GameState.CurrentUser.Blocking != _defaultUser.Blocking;
        }

        private void BtnBlockingPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Blocking = IncreaseAttribute(_createUser.Blocking);
            BtnBlockingMinus.IsEnabled = true;
        }

        private void BtnSlippingMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Slipping = DecreaseAttribute(_createUser.Slipping);
            BtnSlippingMinus.IsEnabled = GameState.CurrentUser.Slipping != _defaultUser.Slipping;
        }

        private void BtnSlippingPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Slipping = IncreaseAttribute(_createUser.Slipping);
            BtnSlippingMinus.IsEnabled = true;
        }

        private void BtnStealthMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Stealth = DecreaseAttribute(_createUser.Stealth);
            BtnStealthMinus.IsEnabled = GameState.CurrentUser.Stealth != _defaultUser.Stealth;
        }

        private void BtnStealthPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Stealth = IncreaseAttribute(_createUser.Stealth);
            BtnStealthMinus.IsEnabled = true;
        }

        #endregion Plus/Minus Buttons

        #region Button-Click Methods

        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (PswdPassword.Password == PswdConfirm.Password)
            {
                if (TxtUsername.Text.Trim().Length >= 4 && PswdPassword.Password.Trim().Length >= 4 && PswdConfirm.Password.Trim().Length >= 4)
                {
                    _createUser.Name = TxtUsername.Text.Trim();
                    if (!GameState.AllUsers.Exists(user => user.Name == _createUser.Name))
                    {
                        if (!string.Equals(_createUser.Name, "Computer", StringComparison.OrdinalIgnoreCase) && !string.Equals(_createUser.Name, "Rathskeller", StringComparison.OrdinalIgnoreCase) && !string.Equals(_createUser.Name, "The Master", StringComparison.OrdinalIgnoreCase))
                        {
                            _createUser.Password = PBKDF2.HashPassword(PswdPassword.Password.Trim());
                            if (await GameState.NewUser(_createUser))
                            {
                                _blnStart = true;
                                ClosePage();
                            }
                        }
                        else
                            GameState.DisplayNotification("That username is reserved and cannot be chosen.", "Assassin");
                    }
                    else
                        GameState.DisplayNotification("That username is taken.", "Assassin");
                }
                else
                    GameState.DisplayNotification("Usernames and passwords must be at least 4 characters in length, excluding leading and trailing spaces.", "Assassin");
            }
            else
                GameState.DisplayNotification("Please ensure the typed passwords match.", "Assassin");
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            DisableMinusButtons();
            TogglePlusButtons(true);
            _createUser = new User(_defaultUser);
            DataContext = _createUser;
            TxtUsername.Clear();
            PswdPassword.Clear();
            PswdConfirm.Clear();
            TxtUsername.Focus();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage()
        {
            if (!_blnStart)
                GameState.GoBack();
            else
            {
                GameState.CurrentUser = new User(_createUser);
                GameState.AllUsers.Add(new User(_createUser));
                GamePage gamePage = new GamePage();
                gamePage.NewUser();
                GameState.Navigate(gamePage);
                GameState.MainWindow.MainFrame.RemoveBackEntry();
            }
        }

        public NewUserPage() => InitializeComponent();

        private void TxtUsername_GotFocus(object sender, RoutedEventArgs e) => Functions.TextBoxGotFocus(sender);

        private void TxtUsername_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Letters);

        private void TxtUsername_Changed(object sender, TextChangedEventArgs e)
        {
            Functions.TextBoxTextChanged(sender, KeyType.Letters);
            CheckButtons();
        }

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void Pswd_TextChanged(object sender, RoutedEventArgs e) => CheckButtons();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _createUser;
            CheckButtons();
            TxtUsername.Focus();
        }

        #endregion Page-Manipulation Methods
    }
}