using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window, INotifyPropertyChanged
    {
        private bool blnStart = false;
        private User createUser = new User();
        private User defaultUser = new User();

        internal MainWindow RefToMainWindow { get; set; }

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
            if (createUser.SkillPoints > 0)
            {
                EnablePlusButtons();
                btnCreate.IsEnabled = false;

                if (createUser.SkillPoints == defaultUser.SkillPoints)
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
            createUser.SkillPoints--;
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
            createUser.SkillPoints++;
            CheckButtons();
            return attribute;
        }

        #endregion Attribute Modification

        #region Plus/Minus Buttons

        private void btnEnduranceMinus_Click(object sender, RoutedEventArgs e)
        {
            createUser.SkillPoints++;
            createUser.CurrentEndurance -= 20;
            createUser.MaximumEndurance -= 20;
            if (createUser.MaximumEndurance == defaultUser.MaximumEndurance)
                btnEnduranceMinus.IsEnabled = false;
            CheckButtons();
        }

        private void btnEndurancePlus_Click(object sender, RoutedEventArgs e)
        {
            createUser.SkillPoints--;
            createUser.CurrentEndurance += 20;
            createUser.MaximumEndurance += 20;
            btnEnduranceMinus.IsEnabled = true;
            CheckButtons();
        }

        private void btnLightWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            createUser.LightWeaponSkill = DecreaseAttribute(createUser.LightWeaponSkill);
            if (createUser.LightWeaponSkill == defaultUser.LightWeaponSkill)
                btnLightWeaponsMinus.IsEnabled = false;
        }

        private void btnLightWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            createUser.LightWeaponSkill = IncreaseAttribute(createUser.LightWeaponSkill);
            btnLightWeaponsMinus.IsEnabled = true;
        }

        private void btnHeavyWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            createUser.HeavyWeaponSkill = DecreaseAttribute(createUser.HeavyWeaponSkill);
            if (createUser.HeavyWeaponSkill == defaultUser.HeavyWeaponSkill)
                btnHeavyWeaponsMinus.IsEnabled = false;
        }

        private void btnHeavyWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            createUser.HeavyWeaponSkill = IncreaseAttribute(createUser.HeavyWeaponSkill);
            btnHeavyWeaponsMinus.IsEnabled = true;
        }

        private void btnTwoHandedWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            createUser.TwoHandedWeaponSkill = DecreaseAttribute(createUser.TwoHandedWeaponSkill);
            if (createUser.TwoHandedWeaponSkill == defaultUser.TwoHandedWeaponSkill)
                btnTwoHandedWeaponsMinus.IsEnabled = false;
        }

        private void btnTwoHandedWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            createUser.TwoHandedWeaponSkill = IncreaseAttribute(createUser.TwoHandedWeaponSkill);
            btnTwoHandedWeaponsMinus.IsEnabled = true;
        }

        private void btnBlockingMinus_Click(object sender, RoutedEventArgs e)
        {
            createUser.Blocking = DecreaseAttribute(createUser.Blocking);
            if (createUser.Blocking == defaultUser.Blocking)
                btnBlockingMinus.IsEnabled = false;
        }

        private void btnBlockingPlus_Click(object sender, RoutedEventArgs e)
        {
            createUser.Blocking = IncreaseAttribute(createUser.Blocking);
            btnBlockingMinus.IsEnabled = true;
        }

        private void btnSlippingMinus_Click(object sender, RoutedEventArgs e)
        {
            createUser.Slipping = DecreaseAttribute(createUser.Slipping);
            if (createUser.Slipping == defaultUser.Slipping)
                btnSlippingMinus.IsEnabled = false;
        }

        private void btnSlippingPlus_Click(object sender, RoutedEventArgs e)
        {
            createUser.Slipping = IncreaseAttribute(createUser.Slipping);
            btnSlippingMinus.IsEnabled = true;
        }

        private void btnStealthMinus_Click(object sender, RoutedEventArgs e)
        {
            createUser.Stealth = DecreaseAttribute(createUser.Stealth);
            if (createUser.Stealth == defaultUser.Stealth)
                btnStealthMinus.IsEnabled = false;
        }

        private void btnStealthPlus_Click(object sender, RoutedEventArgs e)
        {
            createUser.Stealth = IncreaseAttribute(createUser.Stealth);
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
                    createUser.Name = txtUsername.Text.Trim();
                    createUser.Password = PasswordHash.HashPassword(pswdPassword.Password.Trim());
                    if (await GameState.CreateUser(createUser))
                    {
                        blnStart = true;
                        CloseWindow();
                    }
                }
                else
                    MessageBox.Show("Usernames and passwords must be at least 4 characters in length, excluding leading and trailing spaces.", "Assassin", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Please ensure the typed passwords match.", "Assassin", MessageBoxButton.OK);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            DisableMinusButtons();
            EnablePlusButtons();
            createUser = new User(defaultUser);
            DataContext = createUser;
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
            DataContext = createUser;
            CheckButtons();
            txtUsername.Focus();
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUsername.SelectAll();
        }

        private void txtUsername_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key k = e.Key;

            List<bool> keys = GameState.GetListOfKeys(Key.Back, Key.Delete, Key.Home, Key.End, Key.Enter, Key.Tab, Key.Left, Key.Right, Key.Escape, Key.LeftShift, Key.RightShift, Key.LeftAlt, Key.RightAlt, Key.LeftCtrl, Key.RightCtrl);

            if (keys.Any(key => key == true) || Key.A <= k && k <= Key.Z)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtUsername_Changed(object sender, TextChangedEventArgs e)
        {
            txtUsername.Text = new string((from c in txtUsername.Text
                                           where char.IsLetter(c)
                                           select c).ToArray());
            txtUsername.CaretIndex = txtUsername.Text.Length;
            CheckButtons();
        }

        private void pswdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pswdPassword.SelectAll();
        }

        private void pswd_Changed(object sender, RoutedEventArgs e)
        {
            CheckButtons();
        }

        private void pswdConfirm_GotFocus(object sender, RoutedEventArgs e)
        {
            pswdConfirm.SelectAll();
        }

        private void CloseWindow()
        {
            this.Close();
        }

        private void windowNewUser_Closing(object sender, CancelEventArgs e)
        {
            if (!blnStart)
                RefToMainWindow.Show();
            else
            {
                GameState.CurrentUser = new User(createUser);
                GameState.AllUsers.Add(new User(createUser));
                GameWindow gameWindow = new GameWindow();
                gameWindow.RefToMainWindow = RefToMainWindow;
                gameWindow.Show();
                gameWindow.NewUser();
            }
        }

        #endregion Window-Manipulation Methods
    }
}