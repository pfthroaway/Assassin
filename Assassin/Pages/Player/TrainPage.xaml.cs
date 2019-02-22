using Assassin.Classes;
using Assassin.Classes.Entities;
using System.Windows;

namespace Assassin.Pages.Player
{
    /// <summary>Interaction logic for TrainPage.xaml</summary>
    public partial class TrainPage
    {
        private User _originalUser = new User();

        #region Button-Manipulation Methods

        /// <summary>Check which buttons to enable.</summary>
        private void CheckButtons()
        {
            BtnTrain.IsEnabled = GameState.CurrentUser.SkillPoints != _originalUser.SkillPoints;
            if (GameState.CurrentUser.SkillPoints > 0)
            {
                TogglePlusButtons(true);
                if (GameState.CurrentUser.SkillPoints == _originalUser.SkillPoints)
                    DisableMinusButtons();
            }
            else
                TogglePlusButtons(false);
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
            BtnLightWeaponsPlus.IsEnabled = enabled && GameState.CurrentUser.LightWeaponSkill < 90;
            BtnHeavyWeaponsPlus.IsEnabled = enabled && GameState.CurrentUser.HeavyWeaponSkill < 90;
            BtnTwoHandedWeaponsPlus.IsEnabled = enabled && GameState.CurrentUser.TwoHandedWeaponSkill < 90;
            BtnBlockingPlus.IsEnabled = enabled && GameState.CurrentUser.Blocking < 90;
            BtnSlippingPlus.IsEnabled = enabled && GameState.CurrentUser.Slipping < 90;
            BtnStealthPlus.IsEnabled = enabled && GameState.CurrentUser.Stealth < 90;
        }

        #endregion Button-Manipulation Methods

        #region Attribute Modification

        /// <summary>Increases the passed attribute.</summary>
        /// <param name="attribute">Attribute to be increased</param>
        /// <returns>Increased attribute</returns>
        private int IncreaseAttribute(int attribute)
        {
            attribute += 8;
            GameState.CurrentUser.SkillPoints--;
            CheckButtons();
            return attribute;
        }

        /// <summary>Decreases the passed attribute.</summary>
        /// <param name="attribute">Attribute to be decreased</param>
        /// <returns>Decreased attribute</returns>
        private int DecreaseAttribute(int attribute)
        {
            attribute -= 8;
            GameState.CurrentUser.SkillPoints++;
            CheckButtons();
            return attribute;
        }

        #endregion Attribute Modification

        #region Plus/Minus Buttons

        private void BtnEnduranceMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.SkillPoints++;
            GameState.CurrentUser.CurrentEndurance -= 20;
            GameState.CurrentUser.MaximumEndurance -= 20;
            BtnEnduranceMinus.IsEnabled = GameState.CurrentUser.MaximumEndurance != _originalUser.MaximumEndurance;
            CheckButtons();
        }

        private void BtnEndurancePlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.SkillPoints--;
            GameState.CurrentUser.CurrentEndurance += 20;
            GameState.CurrentUser.MaximumEndurance += 20;
            BtnEnduranceMinus.IsEnabled = true;
            CheckButtons();
        }

        private void BtnLightWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.LightWeaponSkill = DecreaseAttribute(GameState.CurrentUser.LightWeaponSkill);
            BtnLightWeaponsMinus.IsEnabled = GameState.CurrentUser.LightWeaponSkill != _originalUser.LightWeaponSkill;
        }

        private void BtnLightWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.LightWeaponSkill = IncreaseAttribute(GameState.CurrentUser.LightWeaponSkill);
            BtnLightWeaponsMinus.IsEnabled = true;
        }

        private void BtnHeavyWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.HeavyWeaponSkill = DecreaseAttribute(GameState.CurrentUser.HeavyWeaponSkill);
            BtnHeavyWeaponsMinus.IsEnabled = GameState.CurrentUser.HeavyWeaponSkill != _originalUser.HeavyWeaponSkill;
        }

        private void BtnHeavyWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.HeavyWeaponSkill = IncreaseAttribute(GameState.CurrentUser.HeavyWeaponSkill);
            BtnHeavyWeaponsMinus.IsEnabled = true;
        }

        private void BtnTwoHandedWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.TwoHandedWeaponSkill = DecreaseAttribute(GameState.CurrentUser.TwoHandedWeaponSkill);
            BtnTwoHandedWeaponsMinus.IsEnabled = GameState.CurrentUser.TwoHandedWeaponSkill != _originalUser.TwoHandedWeaponSkill;
        }

        private void BtnTwoHandedWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.TwoHandedWeaponSkill = IncreaseAttribute(GameState.CurrentUser.TwoHandedWeaponSkill);
            BtnTwoHandedWeaponsMinus.IsEnabled = true;
        }

        private void BtnBlockingMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Blocking = DecreaseAttribute(GameState.CurrentUser.Blocking);
            BtnSlippingMinus.IsEnabled = GameState.CurrentUser.Blocking != _originalUser.Blocking;
        }

        private void BtnBlockingPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Blocking = IncreaseAttribute(GameState.CurrentUser.Blocking);
            BtnBlockingMinus.IsEnabled = true;
        }

        private void BtnSlippingMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Slipping = DecreaseAttribute(GameState.CurrentUser.Slipping);
            BtnSlippingMinus.IsEnabled = GameState.CurrentUser.Slipping != _originalUser.Slipping;
        }

        private void BtnSlippingPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Slipping = IncreaseAttribute(GameState.CurrentUser.Slipping);
            BtnSlippingMinus.IsEnabled = true;
        }

        private void BtnStealthMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Stealth = DecreaseAttribute(GameState.CurrentUser.Stealth);
            BtnStealthMinus.IsEnabled = GameState.CurrentUser.Stealth != _originalUser.Stealth;
        }

        private void BtnStealthPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Stealth = IncreaseAttribute(GameState.CurrentUser.Stealth);
            BtnStealthMinus.IsEnabled = true;
        }

        #endregion Plus/Minus Buttons

        #region Button-Click Methods

        private void BtnTrain_Click(object sender, RoutedEventArgs e) => ClosePage(true);

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        private async void ClosePage(bool save = false)
        {
            if (save)
                await GameState.SaveUser(GameState.CurrentUser);
            else
                GameState.CurrentUser = _originalUser;
            GameState.GoBack();
        }

        public TrainPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _originalUser = new User(GameState.CurrentUser);
            DataContext = GameState.CurrentUser;
            CheckButtons();
        }

        #endregion Page-Manipulation Methods
    }
}