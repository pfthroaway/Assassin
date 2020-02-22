using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Assassin.Models.Items;
using Extensions;
using Extensions.DataTypeHelpers;
using Extensions.Encryption;
using Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assassin.Views.Admin
{
    /// <summary>Interaction logic for AdminUsersPage.xaml</summary>
    public partial class AdminUsersPage
    {
        private bool _blnNewUser;
        private User _selectedUser = new User();

        /// <summary>Assigns all the Controls' data to the _selectedUser <see cref="User"/>.</summary>
        private void AssignSelectedUser(bool hashPassword = false)
        {
            _selectedUser.Name = TxtName.Text.Trim();
            if (hashPassword)
                _selectedUser.Password = PBKDF2.HashPassword(PswdPassword.Password.Trim());

            // character
            _selectedUser.Level = Int32Helper.Parse(TxtLevel.Text.Trim());
            _selectedUser.Experience = Int32Helper.Parse(TxtExperience.Text.Trim());
            _selectedUser.SkillPoints = Int32Helper.Parse(TxtSkillPoints.Text.Trim());
            _selectedUser.Alive = ChkAlive.IsChecked != null && ChkAlive.IsChecked.Value;
            _selectedUser.CurrentEndurance = Int32Helper.Parse(TxtCurrentEndurance.Text.Trim());
            _selectedUser.CurrentLocation = EnumHelper.Parse<SleepLocation>(CmbLocation.SelectedItem.ToString());
            _selectedUser.MaximumEndurance = Int32Helper.Parse(TxtMaximumEndurance.Text.Trim());
            _selectedUser.Hunger = Int32Helper.Parse(TxtHunger.Text.Trim());
            _selectedUser.Thirst = Int32Helper.Parse(TxtThirst.Text.Trim());

            // inventory
            _selectedUser.CurrentWeaponType = EnumHelper.Parse<WeaponType>(CmbCurrentWeapon.SelectedItem.ToString());
            _selectedUser.LightWeapon = (Weapon)CmbLightWeapon.SelectedItem;
            _selectedUser.HeavyWeapon = (Weapon)CmbHeavyWeapon.SelectedItem;
            _selectedUser.TwoHandedWeapon = (Weapon)CmbTwoHWeapon.SelectedItem;
            _selectedUser.Armor = (Armor)CmbArmor.SelectedItem;
            _selectedUser.Potion = (Potion)CmbPotion.SelectedItem;
            _selectedUser.Lockpicks = Int32Helper.Parse(TxtLockpicks.Text.Trim());
            _selectedUser.GoldOnHand = Int32Helper.Parse(TxtGoldOnHand.Text.Trim());
            _selectedUser.GoldInBank = Int32Helper.Parse(TxtGoldInBank.Text.Trim());
            _selectedUser.GoldOnLoan = Int32Helper.Parse(TxtGoldOnLoan.Text.Trim());
            _selectedUser.Shovel = ChkShovel.IsChecked != null && ChkShovel.IsChecked.Value;
            _selectedUser.Lantern = ChkLantern.IsChecked != null && ChkLantern.IsChecked.Value;
            _selectedUser.Amulet = ChkAmulet.IsChecked != null && ChkAmulet.IsChecked.Value;

            // skills
            _selectedUser.LightWeaponSkill = Int32Helper.Parse(TxtLightWeaponSkill.Text.Trim());
            _selectedUser.HeavyWeaponSkill = Int32Helper.Parse(TxtHeavyWeaponSkill.Text.Trim());
            _selectedUser.TwoHandedWeaponSkill = Int32Helper.Parse(TxtTwoHWeaponSkill.Text.Trim());
            _selectedUser.Blocking = Int32Helper.Parse(TxtBlockingSkill.Text.Trim());
            _selectedUser.Slipping = Int32Helper.Parse(TxtSlippingSkill.Text.Trim());
            _selectedUser.Stealth = Int32Helper.Parse(TxtStealthSkill.Text.Trim());

            // henchmen
            _selectedUser.Henchmen.Level1 = Int32Helper.Parse(TxtHenchmenLevel1.Text.Trim());
            _selectedUser.Henchmen.Level2 = Int32Helper.Parse(TxtHenchmenLevel2.Text.Trim());
            _selectedUser.Henchmen.Level3 = Int32Helper.Parse(TxtHenchmenLevel3.Text.Trim());
            _selectedUser.Henchmen.Level4 = Int32Helper.Parse(TxtHenchmenLevel4.Text.Trim());
            _selectedUser.Henchmen.Level5 = Int32Helper.Parse(TxtHenchmenLevel5.Text.Trim());
        }

        /// <summary>Deletes a <see cref="User"/> from Jail if the location was changed.</summary>
        private async void CheckJail()
        {
            if (GameState.CurrentUser.CurrentLocation == SleepLocation.Jail)
            {
                JailedUser jailedUser = GameState.AllJailedUsers.Find(jailed => jailed.Name == GameState.CurrentUser.Name);
                if (await GameState.DatabaseInteraction.FreeFromJail(jailedUser))
                    GameState.AllJailedUsers.Remove(jailedUser);
            }
        }

        ///<summary>Displays information about the current<see cref="User"/>.</summary>
        private void DisplayUser()
        {
            // character
            TxtName.Text = GameState.CurrentUser.Name.ToString();
            TxtLevel.Text = GameState.CurrentUser.Level.ToString();
            TxtExperience.Text = GameState.CurrentUser.Experience.ToString();
            TxtSkillPoints.Text = GameState.CurrentUser.SkillPoints.ToString();
            ChkAlive.IsChecked = GameState.CurrentUser.Alive;
            CmbLocation.SelectedItem = GameState.CurrentUser.CurrentLocation;
            TxtCurrentEndurance.Text = GameState.CurrentUser.CurrentEndurance.ToString();
            TxtMaximumEndurance.Text = GameState.CurrentUser.MaximumEndurance.ToString();
            TxtHunger.Text = GameState.CurrentUser.Hunger.ToString();
            TxtThirst.Text = GameState.CurrentUser.Thirst.ToString();

            // inventory
            CmbCurrentWeapon.SelectedItem = GameState.CurrentUser.CurrentWeaponType.ToString();
            CmbLightWeapon.SelectedItem = GameState.CurrentUser.LightWeapon;
            CmbHeavyWeapon.SelectedItem = GameState.CurrentUser.HeavyWeapon;
            CmbTwoHWeapon.SelectedItem = GameState.CurrentUser.TwoHandedWeapon;
            CmbArmor.SelectedItem = GameState.CurrentUser.Armor;
            CmbPotion.SelectedItem = GameState.CurrentUser.Potion;
            TxtLockpicks.Text = GameState.CurrentUser.Lockpicks.ToString();
            TxtGoldOnHand.Text = GameState.CurrentUser.GoldOnHand.ToString();
            TxtGoldInBank.Text = GameState.CurrentUser.GoldInBank.ToString();
            TxtGoldOnLoan.Text = GameState.CurrentUser.GoldOnLoan.ToString();
            ChkShovel.IsChecked = GameState.CurrentUser.Shovel;
            ChkLantern.IsChecked = GameState.CurrentUser.Lantern;
            ChkAmulet.IsChecked = GameState.CurrentUser.Amulet;

            // skills
            TxtLightWeaponSkill.Text = GameState.CurrentUser.LightWeaponSkill.ToString();
            TxtHeavyWeaponSkill.Text = GameState.CurrentUser.HeavyWeaponSkill.ToString();
            TxtTwoHWeaponSkill.Text = GameState.CurrentUser.TwoHandedWeaponSkill.ToString();
            TxtBlockingSkill.Text = GameState.CurrentUser.Blocking.ToString();
            TxtSlippingSkill.Text = GameState.CurrentUser.Slipping.ToString();
            TxtStealthSkill.Text = GameState.CurrentUser.Stealth.ToString();

            // henchmen
            TxtHenchmenLevel1.Text = GameState.CurrentUser.Henchmen.Level1.ToString();
            TxtHenchmenLevel2.Text = GameState.CurrentUser.Henchmen.Level2.ToString();
            TxtHenchmenLevel3.Text = GameState.CurrentUser.Henchmen.Level3.ToString();
            TxtHenchmenLevel4.Text = GameState.CurrentUser.Henchmen.Level4.ToString();
            TxtHenchmenLevel5.Text = GameState.CurrentUser.Henchmen.Level5.ToString();
        }

        #region Load

        /// <summary>Loads the Admin Users Form.</summary>
        private void LoadAdmin()
        {
            RefreshItemsSource();
            CmbArmor.ItemsSource = GameState.AllArmor;
            CmbPotion.ItemsSource = GameState.AllPotions;
            CmbLightWeapon.ItemsSource = GameState.AllWeapons.Where(wpn => wpn.Type == WeaponType.Light).ToList();
            CmbHeavyWeapon.ItemsSource = GameState.AllWeapons.Where(wpn => wpn.Type == WeaponType.Heavy).ToList();
            CmbTwoHWeapon.ItemsSource = GameState.AllWeapons.Where(wpn => wpn.Type == WeaponType.TwoHanded).ToList();
            Clear();
        }

        #endregion Load

        private void RefreshItemsSource()
        {
            LstUsers.UnselectAll();
            LstUsers.ItemsSource = GameState.AllUsers;
            LstUsers.Items.Refresh();
        }

        #region Save

        /// <summary>Modifies an existing <see cref="User"/> and saves it to the database.</summary>
        private async void ModifyExistingUser()
        {
            // if changing password
            bool blnChangePass = false;
            if (PswdPassword.Password.Trim().Length > 0 && PswdConfirm.Password.Trim().Length > 0 && PswdPassword.Password.Trim() == PswdConfirm.Password.Trim())
                blnChangePass = true;
            else if (PswdPassword.Password.Trim() != PswdConfirm.Password.Trim())
            {
                GameState.DisplayNotification("Your passwords don't match.", "Assassin");
                PswdPassword.Clear();
                PswdConfirm.Clear();
                PswdPassword.Focus();
                return;
            }

            // if changing user name
            bool blnChangeName = false;
            if (TxtName.Text.Trim() != GameState.CurrentUser.Name)
            {
                if (!GameState.AllUsers.Exists(user => user.Name == TxtName.Text.Trim()) &&
                    !string.Equals(TxtName.Text.Trim(), "Computer", StringComparison.OrdinalIgnoreCase) && !string.Equals(TxtName.Text.Trim(), "Rathskeller", StringComparison.OrdinalIgnoreCase) && !string.Equals(TxtName.Text.Trim(), "The Master", StringComparison.OrdinalIgnoreCase))
                    blnChangeName = true;
                else
                {
                    if (string.Equals(TxtName.Text.Trim(), "Computer", StringComparison.OrdinalIgnoreCase) || string.Equals(TxtName.Text.Trim(), "Rathskeller", StringComparison.OrdinalIgnoreCase) || string.Equals(TxtName.Text.Trim(), "The Master"))
                        GameState.DisplayNotification("That username is reserved and cannot be chosen.", "Assassin");
                    else
                        GameState.DisplayNotification("The new username you have chosen is already in use.", "Assassin");
                    TxtName.Clear();
                    TxtName.Focus();
                    return;
                }
            }

            AssignSelectedUser(blnChangePass);

            if (blnChangeName && await GameState.DatabaseInteraction.SaveUser(_selectedUser, TxtName.Text))
            {
                foreach (Guild guild in GameState.AllGuilds)
                {
                    if (guild.HasMember(GameState.CurrentUser))
                    {
                        await GameState.MemberLeavesGuild(GameState.CurrentUser, guild);
                        await GameState.MemberJoinsGuild(_selectedUser, guild);
                    }
                    if (await GameState.DatabaseInteraction.HasAppliedToGuild(GameState.CurrentUser, guild))
                    {
                        await GameState.DatabaseInteraction.DenyGuildApplication(GameState.CurrentUser, guild);
                        await GameState.DatabaseInteraction.ApplyToGuild(_selectedUser, guild);
                    }

                    if (guild.Master == GameState.CurrentUser.Name)
                        guild.Master = _selectedUser.Name;
                }
            }
            else if (!blnChangeName)
                await GameState.DatabaseInteraction.SaveUser(_selectedUser);

            GameState.AllUsers.Replace(GameState.CurrentUser, _selectedUser);

            if (GameState.CurrentUser.CurrentLocation == SleepLocation.Jail && _selectedUser.CurrentLocation != SleepLocation.Jail)
                CheckJail();

            Clear();
            RefreshItemsSource();
        }

        /// <summary>Checks to make sure all the fields have text and then saves the <see cref="User"/>.</summary>
        private void Save()
        {
            if (LstUsers.SelectedIndex >= 0 || _blnNewUser)
            {
                if (TxtBlockingSkill.Text.Length > 0 && TxtCurrentEndurance.Text.Length > 0 && TxtExperience.Text.Length > 0 && TxtGoldInBank.Text.Length > 0 && TxtGoldOnHand.Text.Length > 0 && TxtGoldOnLoan.Text.Length > 0 && TxtHeavyWeaponSkill.Text.Length > 0 && TxtHenchmenLevel1.Text.Length > 0 && TxtHenchmenLevel2.Text.Length > 0 && TxtHenchmenLevel3.Text.Length > 0 && TxtHenchmenLevel4.Text.Length > 0 && TxtHenchmenLevel5.Text.Length > 0 && TxtLevel.Text.Length > 0 && TxtLightWeaponSkill.Text.Length > 0 && TxtLockpicks.Text.Length > 0 && TxtMaximumEndurance.Text.Length > 0 && TxtName.Text.Length > 0 && TxtSkillPoints.Text.Length > 0 && TxtSlippingSkill.Text.Length > 0 && TxtStealthSkill.Text.Length > 0 && TxtTwoHWeaponSkill.Text.Length > 0 && CmbArmor.SelectedIndex > -1 && CmbLocation.SelectedIndex > -1 && CmbPotion.SelectedIndex > -1 && CmbCurrentWeapon.SelectedIndex > -1 && CmbLightWeapon.SelectedIndex > -1 && CmbHeavyWeapon.SelectedIndex > -1 && CmbTwoHWeapon.SelectedIndex > -1)
                {
                    if (_blnNewUser)
                        SaveNewUser();
                    else
                        ModifyExistingUser();
                }
                else
                {
                    GameState.DisplayNotification("Please ensure all fields are properly filled.", "Assassin");
                } // end save
            }
            else if (LstUsers.SelectedIndex < 0)
            {
                GameState.DisplayNotification("Please select a user to edit or create a new user before saving.", "Assassin");
            }
        }

        /// <summary>Saves a new <see cref="User"/> to the database.</summary>
        private async void SaveNewUser()
        {
            if (PswdPassword.Password.Trim().Length > 0 && PswdConfirm.Password.Trim().Length > 0)
            {
                if (PswdPassword.Password.Trim() == PswdConfirm.Password.Trim())
                {
                    if (!GameState.AllUsers.Exists(user => user.Name == TxtName.Text.Trim()))
                    {
                        if (!string.Equals(_selectedUser.Name, "Computer", StringComparison.OrdinalIgnoreCase) && !string.Equals(_selectedUser.Name, "Rathskeller", StringComparison.OrdinalIgnoreCase) && !string.Equals(_selectedUser.Name, "The Master", StringComparison.OrdinalIgnoreCase))
                        {
                            AssignSelectedUser(true);
                            if (await GameState.NewUser(_selectedUser))
                            {
                                GameState.DisplayNotification("New user successfully created.", "Assassin");
                                Clear();
                                RefreshItemsSource();
                            }
                        }
                        else
                            GameState.DisplayNotification("That username is reserved and cannot be chosen.", "Assassin");
                    }
                    else
                    {
                        GameState.DisplayNotification("The username you have chosen is already in use.", "Assassin");
                        TxtName.Clear();
                        TxtName.Focus();
                    }
                }
                else
                {
                    GameState.DisplayNotification("Your passwords don't match.", "Assassin");
                    PswdPassword.Clear();
                    PswdConfirm.Clear();
                    PswdPassword.Focus();
                }
            }
            else
                GameState.DisplayNotification("Please enter a password and confirm it.", "Assassin");
        }

        /// <summary>Sets up a new <see cref="User"/>.</summary>
        private void SetUpNewUser()
        {
            Clear();
            ToggleControls(true);
            _blnNewUser = true;

            GameState.CurrentUser = new User();
            _selectedUser = new User();
            DisplayUser();
            TxtName.Focus();
        }

        #endregion Save

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private void BtnClear_Click(object sender, RoutedEventArgs e) => Clear();

        private async void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (LstUsers.SelectedIndex >= 0)
            {
                if (GameState.YesNoNotification($"Are you sure you want to delete {GameState.CurrentUser.Name}?", "Assassin"))
                {
                    if (await GameState.DatabaseInteraction.DeleteUser(GameState.CurrentUser))
                    {
                        foreach (var guild in GameState.AllGuilds)   // remove member from all guilds
                        {
                            guild.Members.Remove(GameState.CurrentUser.Name);
                            await GameState.MemberLeavesGuild(GameState.CurrentUser, guild);
                            await GameState.DatabaseInteraction.DenyGuildApplication(GameState.CurrentUser, guild);
                            if (guild.Master == GameState.CurrentUser.Name)
                                guild.Master = guild.DefaultMaster;
                        }
                        List<Message> messages = await GameState.DatabaseInteraction.LoadMessages(GameState.CurrentUser);
                        foreach (var message in messages)
                            await GameState.DatabaseInteraction.DeleteMessage(message);
                        GameState.AllUsers.Remove(GameState.CurrentUser);
                        GameState.DisplayNotification("User successfully deleted.", "Assassin");

                        Clear();
                        RefreshItemsSource();
                    }
                }
            }
            else
                GameState.DisplayNotification("Please select a user to delete.", "Assassin");
        }

        private void BtnNewUser_Click(object sender, RoutedEventArgs e) => SetUpNewUser();

        private void BtnSave_Click(object sender, RoutedEventArgs e) => Save();

        private void LstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstUsers.SelectedIndex >= 0)
            {
                GameState.CurrentUser = GameState.AllUsers.Find(user => user.Name == LstUsers.SelectedItem.ToString());
                _selectedUser = new User(GameState.CurrentUser);
                DisplayUser();
                ToggleControls(true);
                BtnDeleteUser.IsEnabled = true;
            }
            else
                BtnDeleteUser.IsEnabled = false;
        }

        #endregion Click

        #region Input Manipulation

        /// <summary>Clears all the fields on the form.</summary>
        private void Clear()
        {
            _blnNewUser = false;
            LstUsers.UnselectAll();

            // character
            TxtName.Clear();
            TxtLevel.Clear();
            TxtExperience.Clear();
            TxtSkillPoints.Clear();
            ChkAlive.IsChecked = false;
            CmbLocation.SelectedItem = "Streets";
            TxtCurrentEndurance.Clear();
            TxtMaximumEndurance.Clear();
            TxtHunger.Clear();
            TxtThirst.Clear();

            // inventory
            CmbCurrentWeapon.SelectedIndex = 0;
            CmbLightWeapon.SelectedIndex = 0;
            CmbHeavyWeapon.SelectedIndex = 0;
            CmbTwoHWeapon.SelectedIndex = 0;
            CmbArmor.SelectedIndex = 0;
            CmbPotion.SelectedIndex = 0;
            TxtLockpicks.Clear();
            TxtGoldOnHand.Clear();
            TxtGoldInBank.Clear();
            TxtGoldOnLoan.Clear();
            ChkShovel.IsChecked = false;
            ChkLantern.IsChecked = false;
            ChkAmulet.IsChecked = false;

            // stats
            TxtLightWeaponSkill.Clear();
            TxtHeavyWeaponSkill.Clear();
            TxtTwoHWeaponSkill.Clear();
            TxtBlockingSkill.Clear();
            TxtSlippingSkill.Clear();
            TxtStealthSkill.Clear();

            // henchmen
            TxtHenchmenLevel1.Clear();
            TxtHenchmenLevel2.Clear();
            TxtHenchmenLevel3.Clear();
            TxtHenchmenLevel4.Clear();
            TxtHenchmenLevel5.Clear();

            // password change fields
            PswdPassword.Clear();
            PswdConfirm.Clear();

            ToggleControls(false);
        }

        private void HandleIntTextBox(ref object sender, int maxValue)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text.Length > 0 && Int32Helper.Parse(txt.Text.Trim()) > maxValue)
                txt.Text = maxValue.ToString();
        }

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        /// <summary>Toggles all the Controls on the form.</summary>
        private void ToggleControls(bool enabled)
        {
            // character
            TxtName.IsEnabled = enabled;
            TxtLevel.IsEnabled = enabled;
            TxtExperience.IsEnabled = enabled;
            TxtSkillPoints.IsEnabled = enabled;
            ChkAlive.IsEnabled = enabled;
            CmbLocation.IsEnabled = enabled;
            TxtCurrentEndurance.IsEnabled = enabled;
            TxtMaximumEndurance.IsEnabled = enabled;
            TxtHunger.IsEnabled = enabled;
            TxtThirst.IsEnabled = enabled;

            // inventory
            CmbCurrentWeapon.IsEnabled = enabled;
            CmbLightWeapon.IsEnabled = enabled;
            CmbLightWeapon.IsEnabled = enabled;
            CmbHeavyWeapon.IsEnabled = enabled;
            CmbHeavyWeapon.IsEnabled = enabled;
            CmbTwoHWeapon.IsEnabled = enabled;
            CmbTwoHWeapon.IsEnabled = enabled;
            CmbArmor.IsEnabled = enabled;
            CmbArmor.IsEnabled = enabled;
            CmbPotion.IsEnabled = enabled;
            CmbPotion.IsEnabled = enabled;
            TxtLockpicks.IsEnabled = enabled;
            TxtGoldOnHand.IsEnabled = enabled;
            TxtGoldInBank.IsEnabled = enabled;
            TxtGoldOnLoan.IsEnabled = enabled;
            ChkShovel.IsEnabled = enabled;
            ChkLantern.IsEnabled = enabled;
            ChkAmulet.IsEnabled = enabled;

            // stats
            TxtLightWeaponSkill.IsEnabled = enabled;
            TxtHeavyWeaponSkill.IsEnabled = enabled;
            TxtTwoHWeaponSkill.IsEnabled = enabled;
            TxtBlockingSkill.IsEnabled = enabled;
            TxtSlippingSkill.IsEnabled = enabled;
            TxtStealthSkill.IsEnabled = enabled;

            // henchmen
            TxtHenchmenLevel1.IsEnabled = enabled;
            TxtHenchmenLevel2.IsEnabled = enabled;
            TxtHenchmenLevel3.IsEnabled = enabled;
            TxtHenchmenLevel4.IsEnabled = enabled;
            TxtHenchmenLevel5.IsEnabled = enabled;

            // password change fields
            PswdPassword.IsEnabled = enabled;
            PswdConfirm.IsEnabled = enabled;

            // buttons
            BtnClear.IsEnabled = enabled;
            BtnSave.IsEnabled = enabled;
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e) => Functions.TextBoxGotFocus(sender);

        private void TxtNum_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Integers);

        private void TxtSkill_TextChanged(object sender, TextChangedEventArgs e) => HandleIntTextBox(ref sender, 90);

        private void TxtLevel_TextChanged(object sender, TextChangedEventArgs e) => HandleIntTextBox(ref sender, 11);

        private void TxtEndurance_TextChanged(object sender, TextChangedEventArgs e) => HandleIntTextBox(ref sender, 980);

        private void TxtExperience_TextChanged(object sender, TextChangedEventArgs e) => HandleIntTextBox(ref sender, 100);

        private void TxtHungerThirst_TextChanged(object sender, TextChangedEventArgs e) => HandleIntTextBox(ref sender, 24);

        #endregion Input Manipulation

        #region Page-Manipulation Methods

        public AdminUsersPage()
        {
            InitializeComponent();
            CmbCurrentWeapon.ItemsSource = Enum.GetValues(typeof(WeaponType));
            CmbLocation.ItemsSource = Enum.GetValues(typeof(SleepLocation));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAdmin();
            if (LstUsers.Items.Count > 0)
                LstUsers.SelectedIndex = 0;
        }

        #endregion Page-Manipulation Methods
    }
}