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
                EnablePlusButtons();
                if (GameState.CurrentUser.SkillPoints == _originalUser.SkillPoints)
                    DisableMinusButtons();
            }
            else
                DisablePlusButtons();
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

        /// <summary>Disables Plus buttons.</summary>
        private void DisablePlusButtons()
        {
            BtnEndurancePlus.IsEnabled = false;
            BtnLightWeaponsPlus.IsEnabled = false;
            BtnHeavyWeaponsPlus.IsEnabled = false;
            BtnTwoHandedWeaponsPlus.IsEnabled = false;
            BtnBlockingPlus.IsEnabled = false;
            BtnSlippingPlus.IsEnabled = false;
            BtnStealthPlus.IsEnabled = false;
        }

        /// <summary>Enables Plus buttons.</summary>
        private void EnablePlusButtons()
        {
            BtnEndurancePlus.IsEnabled = true;
            BtnLightWeaponsPlus.IsEnabled = true;
            BtnHeavyWeaponsPlus.IsEnabled = true;
            BtnTwoHandedWeaponsPlus.IsEnabled = true;
            BtnBlockingPlus.IsEnabled = true;
            BtnSlippingPlus.IsEnabled = true;
            BtnStealthPlus.IsEnabled = true;
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
            if (GameState.CurrentUser.MaximumEndurance == _originalUser.MaximumEndurance)
                BtnEnduranceMinus.IsEnabled = false;
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
            if (GameState.CurrentUser.LightWeaponSkill == _originalUser.LightWeaponSkill)
                BtnLightWeaponsMinus.IsEnabled = false;
        }

        private void BtnLightWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.LightWeaponSkill = IncreaseAttribute(GameState.CurrentUser.LightWeaponSkill);
            BtnLightWeaponsMinus.IsEnabled = true;
        }

        private void BtnHeavyWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.HeavyWeaponSkill = DecreaseAttribute(GameState.CurrentUser.HeavyWeaponSkill);
            if (GameState.CurrentUser.HeavyWeaponSkill == _originalUser.HeavyWeaponSkill)
                BtnHeavyWeaponsMinus.IsEnabled = false;
        }

        private void BtnHeavyWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.HeavyWeaponSkill = IncreaseAttribute(GameState.CurrentUser.HeavyWeaponSkill);
            BtnHeavyWeaponsMinus.IsEnabled = true;
        }

        private void BtnTwoHandedWeaponsMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.TwoHandedWeaponSkill = DecreaseAttribute(GameState.CurrentUser.TwoHandedWeaponSkill);
            if (GameState.CurrentUser.TwoHandedWeaponSkill == _originalUser.TwoHandedWeaponSkill)
                BtnTwoHandedWeaponsMinus.IsEnabled = false;
        }

        private void BtnTwoHandedWeaponsPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.TwoHandedWeaponSkill = IncreaseAttribute(GameState.CurrentUser.TwoHandedWeaponSkill);
            BtnTwoHandedWeaponsMinus.IsEnabled = true;
        }

        private void BtnBlockingMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Blocking = DecreaseAttribute(GameState.CurrentUser.Blocking);
            if (GameState.CurrentUser.Blocking == _originalUser.Blocking)
                BtnBlockingMinus.IsEnabled = false;
        }

        private void BtnBlockingPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Blocking = IncreaseAttribute(GameState.CurrentUser.Blocking);
            BtnBlockingMinus.IsEnabled = true;
        }

        private void BtnSlippingMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Slipping = DecreaseAttribute(GameState.CurrentUser.Slipping);
            if (GameState.CurrentUser.Slipping == _originalUser.Slipping)
                BtnSlippingMinus.IsEnabled = false;
        }

        private void BtnSlippingPlus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Slipping = IncreaseAttribute(GameState.CurrentUser.Slipping);
            BtnSlippingMinus.IsEnabled = true;
        }

        private void BtnStealthMinus_Click(object sender, RoutedEventArgs e)
        {
            GameState.CurrentUser.Stealth = DecreaseAttribute(GameState.CurrentUser.Stealth);
            if (GameState.CurrentUser.Stealth == _originalUser.Stealth)
                BtnStealthMinus.IsEnabled = false;
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
            GameState.CalculateScale(Grid);
            _originalUser = new User(GameState.CurrentUser);
            DataContext = GameState.CurrentUser;
            CheckButtons();
        }

        #endregion Page-Manipulation Methods
    }
}