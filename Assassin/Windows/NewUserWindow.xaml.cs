using Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin
{
    /// <summary>Interaction logic for NewUserWindow.xaml</summary>
    public partial class NewUserWindow : INotifyPropertyChanged
    {
        private bool _blnStart;
        private User _createUser = new User();
        private readonly User _defaultUser = new User();

        internal MainWindow RefToMainWindow { private get; set; }

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Button-Manipulation Methods

        private void CheckButtons()
        {
            if (_createUser.SkillPoints > 0)
            {
                EnablePlusButtons();
                btnCreate.IsEnabled = false;

                if (_createUser.SkillPoints == _defaultUser.SkillPoints)
                {
                    DisableMinusButtons();
                    btnReset.IsEnabled = false;
                }
                else
                    btnReset.IsEnabled = true;
            }
            else
            {
                DisablePlusButtons();
                btnReset.IsEnabled = true;

                if (txtUsername.Text.Length >= 1 && pswdConfirm.Password.Length >= 1 && pswdPassword.Password.Length >= 1)
                    btnCreate.IsEnabled = true;
                else
                    btnCreate.IsEnabled = false;
            }
        }

        /// <summary>
        /// Disable Minus buttons.
        /// </summary>
        private void DisableMinusButtons()

        {
            btnEnduranceMinus.IsEnabled = false;
            btnLightWeaponsMinus.IsEnabled = false;
            btnHeavyWeaponsMinus.IsEnabled = false;
            btnTwoHandedWeaponsMinus.IsEnabled = false;
            btnBlockingMinus.IsEnabled = false;
            btnSlippingMinus.IsEnabled = false;
            btnStealthMinus.IsEnabled = false;
        }

        /// <summary>
        /// Disables Plus buttons.
        /// </summary>
        private void DisablePlusButtons()
        {
            btnEndurancePlus.IsEnabled = false;
            btnLightWeaponsPlus.IsEnabled = false;
            btnHeavyWeaponsPlus.IsEnabled = false;
            btnTwoHandedWeaponsPlus.IsEnabled = false;
            btnBlockingPlus.IsEnabled = false;
            btnSlippingPlus.IsEnabled = false;
            btnStealthPlus.IsEnabled = false;
        }

        /// <summary>
        /// Enables Plus buttons.
        /// </summary>
        private void EnablePlusButtons()
        {
            btnEndurancePlus.IsEnabled = true;
            btnLightWeaponsPlus.IsEnabled = true;
            btnHeavyWeaponsPlus.IsEnabled = true;
            btnTwoHandedWeaponsPlus.IsEnabled = true;
            btnBlockingPlus.IsEnabled = true;
            btnSlippingPlus.IsEnabled = true;
            btnStealthPlus.IsEnabled = true;
        }

        #endregion Button-Manipulation Methods

        #region Attribute Modification

        /// <summary>
        /// Increases the passed attribute.
        /// </summary>
        /// <param name="attribute">Attribute to be increased</param>
        /// <returns>Increased attribute</returns>
        private int IncreaseAttribute(int attribute)
        {
            attribute += 8;
            _createUser.SkillPoints--;
            CheckButtons();
            return attribute;
        }

        /// <summary>
        /// Decreases the passed attribute.
        /// </summary>
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

        private void btnEnduranceMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.SkillPoints++;
            _createUser.CurrentEndurance -= 20;
            _createUser.MaximumEndurance -= 20;
            if (_createUser.MaximumEndurance == _defaultUser.MaximumEndurance)
                btnEnduranceMinus.IsEnabled = false;
            CheckButtons();
        }

        private void btnEndurancePlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.SkillPoints--;
            _createUser.CurrentEndurance += 20;
            _createUser.MaximumEndurance += 20;
            btnEnduranceMinus.IsEnabled = true;
            CheckButtons();
        }

        private void btnLightWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.LightWeaponSkill = DecreaseAttribute(_createUser.LightWeaponSkill);
            if (_createUser.LightWeaponSkill == _defaultUser.LightWeaponSkill)
                btnLightWeaponsMinus.IsEnabled = false;
        }

        private void btnLightWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.LightWeaponSkill = IncreaseAttribute(_createUser.LightWeaponSkill);
            btnLightWeaponsMinus.IsEnabled = true;
        }

        private void btnHeavyWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.HeavyWeaponSkill = DecreaseAttribute(_createUser.HeavyWeaponSkill);
            if (_createUser.HeavyWeaponSkill == _defaultUser.HeavyWeaponSkill)
                btnHeavyWeaponsMinus.IsEnabled = false;
        }

        private void btnHeavyWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.HeavyWeaponSkill = IncreaseAttribute(_createUser.HeavyWeaponSkill);
            btnHeavyWeaponsMinus.IsEnabled = true;
        }

        private void btnTwoHandedWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.TwoHandedWeaponSkill = DecreaseAttribute(_createUser.TwoHandedWeaponSkill);
            if (_createUser.TwoHandedWeaponSkill == _defaultUser.TwoHandedWeaponSkill)
                btnTwoHandedWeaponsMinus.IsEnabled = false;
        }

        private void btnTwoHandedWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.TwoHandedWeaponSkill = IncreaseAttribute(_createUser.TwoHandedWeaponSkill);
            btnTwoHandedWeaponsMinus.IsEnabled = true;
        }

        private void btnBlockingMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Blocking = DecreaseAttribute(_createUser.Blocking);
            if (_createUser.Blocking == _defaultUser.Blocking)
                btnBlockingMinus.IsEnabled = false;
        }

        private void btnBlockingPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Blocking = IncreaseAttribute(_createUser.Blocking);
            btnBlockingMinus.IsEnabled = true;
        }

        private void btnSlippingMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Slipping = DecreaseAttribute(_createUser.Slipping);
            if (_createUser.Slipping == _defaultUser.Slipping)
                btnSlippingMinus.IsEnabled = false;
        }

        private void btnSlippingPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Slipping = IncreaseAttribute(_createUser.Slipping);
            btnSlippingMinus.IsEnabled = true;
        }

        private void btnStealthMinus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Stealth = DecreaseAttribute(_createUser.Stealth);
            if (_createUser.Stealth == _defaultUser.Stealth)
                btnStealthMinus.IsEnabled = false;
        }

        private void btnStealthPlus_Click(object sender, RoutedEventArgs e)
        {
            _createUser.Stealth = IncreaseAttribute(_createUser.Stealth);
            btnStealthMinus.IsEnabled = true;
        }

        #endregion Plus/Minus Buttons

        #region Button-Click Methods

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (pswdPassword.Password == pswdConfirm.Password)
            {
                if (txtUsername.Text.Trim().Length >= 4 && pswdPassword.Password.Trim().Length >= 4 && pswdConfirm.Password.Trim().Length >= 4)
                {
                    _createUser.Name = txtUsername.Text.Trim();
                    _createUser.Password = PasswordHash.HashPassword(pswdPassword.Password.Trim());
                    if (await GameState.CreateUser(_createUser))
                    {
                        _blnStart = true;
                        CloseWindow();
                    }
                }
                else
                    new Notification("Usernames and passwords must be at least 4 characters in length, excluding leading and trailing spaces.", "Assassin", NotificationButtons.OK, this).ShowDialog();
            }
            else
                new Notification("Please ensure the typed passwords match.", "Assassin", NotificationButtons.OK, this).ShowDialog();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            DisableMinusButtons();
            EnablePlusButtons();
            _createUser = new User(_defaultUser);
            DataContext = _createUser;
            txtUsername.Clear();
            pswdPassword.Clear();
            pswdConfirm.Clear();
            txtUsername.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        public NewUserWindow()
        {
            InitializeComponent();
            DataContext = _createUser;
            CheckButtons();
            txtUsername.Focus();
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.TextBoxGotFocus(sender);
        }

        private void txtUsername_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Functions.PreviewKeyDown(e, KeyType.Letters);
        }

        private void txtUsername_Changed(object sender, TextChangedEventArgs e)
        {
            Functions.TextBoxTextChanged(sender, KeyType.Letters);
            CheckButtons();
        }

        private void pswd_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void pswd_TextChanged(object sender, RoutedEventArgs e)
        {
            CheckButtons();
        }

        private void CloseWindow()
        {
            this.Close();
        }

        private void windowNewUser_Closing(object sender, CancelEventArgs e)
        {
            if (!_blnStart)
                RefToMainWindow.Show();
            else
            {
                GameState.CurrentUser = new User(_createUser);
                GameState.AllUsers.Add(new User(_createUser));
                GameWindow gameWindow = new GameWindow { RefToMainWindow = RefToMainWindow };
                gameWindow.Show();
                gameWindow.NewUser();
            }
        }

        #endregion Window-Manipulation Methods
    }
}