using Assassin.Classes;
using Assassin.Classes.Enums;
using Assassin.Classes.Items;
using Assassin.Pages.City;
using Extensions;
using Extensions.DataTypeHelpers;
using System;
using System.ComponentModel;
using System.Windows;

namespace Assassin.Pages.Battle
{
    /// <summary>Interaction logic for BattlePage.xaml</summary>
    public partial class BattlePage : INotifyPropertyChanged
    {
        private int _playerStamina = 20;
        private int _enemyStamina = 20;
        private Stance _playerStance, _enemyStance;
        private bool _battleOver;
        private string _previousPage = "";

        /// <summary>Sets the previous Page.</summary>
        /// <param name="windowName">Previous Page name</param>
        internal void SetPreviousPage(string windowName) => _previousPage = windowName;

        #region Display Manipulation

        /// <summary>Checks which buttons to be enabled.</summary>
        private void CheckButtons()
        {
            if (PlayerStamina > 0)
                EnableButtons();
            else
                BtnDefend.IsEnabled = true;
        }

        /// <summary>Gets a text status for a stamina value.</summary>
        /// <param name="stamina">Stamina amount</param>
        /// <returns>Text based on stamina</returns>
        private string GetStaminaText(int stamina)
        {
            switch (stamina)
            {
                case 19:
                case 20:
                    return "Vigorous";

                case 17:
                case 18:
                    return "Robust";

                case 15:
                case 16:
                    return "Stalwart";

                case 13:
                case 14:
                    return "Beat";

                case 11:
                case 12:
                    return "Shaky";

                case 9:
                case 10:
                    return "Spent";

                case 7:
                case 8:
                    return "Bushed";

                case 5:
                case 6:
                    return "Weary";

                case 3:
                case 4:
                    return "Tired";

                case 0:
                case 1:
                case 2:
                    return "Exhausted";

                default:
                    return "BROKEN";
            }
        }

        #endregion Display Manipulation

        #region Properties

        internal AssassinationPage RefToAssassinationPage { get; set; }
        internal JobsPage RefToJobsPage { get; set; }

        public int PlayerStamina
        {
            get => _playerStamina;
            set { _playerStamina = value; OnPropertyChanged("PlayerStaminaToString"); }
        }

        public int EnemyStamina
        {
            get => _enemyStamina;
            set { _enemyStamina = value; OnPropertyChanged("EnemyStaminaToString"); }
        }

        public string PlayerStaminaToString => GetStaminaText(PlayerStamina);

        public string EnemyStaminaToString => GetStaminaText(EnemyStamina);

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Binds information to labels.</summary>
        private void BindLabels()
        {
            GrpUser.DataContext = GameState.CurrentUser;
            LblPlayerStatus.DataContext = this;
            GrpEnemy.DataContext = GameState.CurrentEnemy;
            LblEnemyStatus.DataContext = this;

            Surprise();
        }

        public void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        #region Button Manipulation

        /// <summary>Disable all the buttons.</summary>
        private void DisableButtons()
        {
            BtnAttack.IsEnabled = false;
            BtnBerserk.IsEnabled = false;
            BtnFlee.IsEnabled = false;
            BtnLunge.IsEnabled = false;
            BtnParry.IsEnabled = false;
            BtnQuickCombat.IsEnabled = false;
        }

        /// <summary>Enable all the buttons.</summary>
        private void EnableButtons()
        {
            BtnAttack.IsEnabled = true;
            BtnFlee.IsEnabled = true;
            BtnLunge.IsEnabled = true;
            BtnParry.IsEnabled = true;
            BtnDefend.IsEnabled = true;
            BtnQuickCombat.IsEnabled = true;
            BtnBerserk.IsEnabled = PlayerStamina >= 2;
        }

        #endregion Button Manipulation

        #region Battle Management

        private void Surprise()
        {
            int surprise = Functions.GenerateRandomNumber(1, 100);
            if (surprise <= GameState.CurrentUser.Stealth)
            {
                Functions.AddTextToTextBox(TxtBattle, "You surprise your opponent!");
                PlayerInflictsDamage(GameState.CurrentUser.SelectedWeapon.Damage, GameState.CurrentEnemy.Armor.Defense);
            }
        }

        /// <summary>A new round of battle.</summary>
        private void NewRound()
        {
            int playerFirst = Functions.GenerateRandomNumber(1, 100);
            int enemyFirst = Functions.GenerateRandomNumber(1, 100);
            _enemyStance = EnemyStance();

            DisableButtons();
            BtnDefend.IsEnabled = false;

            if (playerFirst >= enemyFirst) //if player goes first
            {
                PlayerTurn();

                if (!_battleOver)
                    EnemyTurn();
            }
            else
            {
                EnemyTurn();

                if (!_battleOver)
                    PlayerTurn();
            }
            CheckButtons();
            if (GameState.CurrentEnemy.CurrentEndurance <= 0)
                WinBattle();
            else if (GameState.CurrentUser.CurrentEndurance <= 0)
                LoseBattle();
        }

        /// <summary>The Player's turn.</summary>
        private void PlayerTurn()
        {
            if (_playerStance != Stance.Defend && _playerStance != Stance.Flee)
                PlayerAttack();
            else if (_playerStance == Stance.Flee)
            {
                if (PlayerFlee())
                {
                    Functions.AddTextToTextBox(TxtBattle, "You successfully fled the battle.");
                    EndBattle();
                }
            }
            else if (_playerStance == Stance.Defend)
                PlayerStamina++;
            if (PlayerStamina > 20)
                PlayerStamina = 20;
        }

        /// <summary>The Player attacks.</summary>
        private void PlayerAttack()
        {
            int playerDamage = GameState.CurrentUser.SelectedWeapon.Damage;
            int playerDefense = GameState.CurrentUser.Armor.Defense;
            int enemyDamage = GameState.CurrentEnemy.Weapon.Damage;
            int enemyDefense = GameState.CurrentEnemy.Armor.Defense;

            switch (_enemyStance)
            {
                case Stance.Defend:
                    enemyDefense *= 2;
                    break;

                case Stance.Lunge:
                    enemyDamage = Int32Helper.Parse(enemyDamage * 1.5m);
                    enemyDefense = Int32Helper.Parse(enemyDefense * 0.5m);
                    break;

                case Stance.Berserk:
                    enemyDamage *= 2;
                    break;
            }

            switch (_playerStance)
            {
                case Stance.Normal:
                    PlayerStamina--;
                    break;

                case Stance.Berserk:
                    playerDamage *= 2;
                    PlayerStamina -= 2;
                    break;

                case Stance.Lunge:
                    playerDamage = Int32Helper.Parse(playerDamage * 1.5m);
                    playerDefense = Int32Helper.Parse(playerDefense * 0.5m);
                    PlayerStamina--;
                    break;
            }

            int toHit = Functions.GenerateRandomNumber(10, GameState.CurrentUser.SelectedWeaponSkill);
            int actualHit = Functions.GenerateRandomNumber(10, 90);

            if (toHit >= actualHit) // then hit
            {
                if (_enemyStance == Stance.Parry)
                {
                    int parry = Functions.GenerateRandomNumber(10, GameState.CurrentEnemy.WeaponSkill);
                    int parryDefend = Functions.GenerateRandomNumber(10, GameState.CurrentUser.SelectedWeaponSkill);

                    if (parry >= parryDefend) //enemy successfully parries
                    {
                        int actualDamage = Functions.GenerateRandomNumber(1, enemyDamage);
                        int actualDefend = Functions.GenerateRandomNumber(1, playerDefense);
                        string strParry = "The " + GameState.CurrentEnemy.Name + " parries your attack and attacks you for " + actualDamage + " damage. ";
                        if (actualDamage > actualDefend) //player actually takes damage
                            Functions.AddTextToTextBox(TxtBattle, strParry + "Your armor absorbs " + actualDefend + " damage. " + GameState.CurrentUser.TakeDamage(actualDamage - actualDefend));
                        else
                            Functions.AddTextToTextBox(TxtBattle, strParry + "Your armor absorbs all the damage.");
                    }
                    else
                        PlayerInflictsDamage(playerDamage, enemyDefense);
                }
                else
                {
                    int enemyBlocks = Functions.GenerateRandomNumber(10, GameState.CurrentEnemy.Blocking);
                    if (enemyBlocks >= GameState.CurrentEnemy.Blocking)
                        Functions.AddTextToTextBox(TxtBattle, "The " + GameState.CurrentEnemy.Name + " blocks your attack.");
                    else
                        PlayerInflictsDamage(playerDamage, enemyDefense);
                }
            }
            else
                Functions.AddTextToTextBox(TxtBattle, "You miss.");
        }

        /// <summary>Player inflicts damage on Enemy.</summary>
        /// <param name="playerDamage">Maximum damage the player can inflict</param>
        /// <param name="enemyDefense">Maximum damage the enemy can defend against</param>
        private void PlayerInflictsDamage(int playerDamage, int enemyDefense)
        {
            int actualDamage = Functions.GenerateRandomNumber(1, playerDamage);
            int actualDefend = Functions.GenerateRandomNumber(1, enemyDefense);
            string attack = "You attack the " + GameState.CurrentEnemy.Name + " for " + actualDamage + ". ";
            if (actualDamage > actualDefend)
                Functions.AddTextToTextBox(TxtBattle, attack + "Their armor absorbs " + actualDefend + " damage. " + GameState.CurrentEnemy.TakeDamage(actualDamage - actualDefend));
            else
                Functions.AddTextToTextBox(TxtBattle, attack + "Their armor absorbs all of it.");
        }

        /// <summary>Sets the Enemy's stance.</summary>
        /// <returns>Returns stance</returns>
        private Stance EnemyStance()
        {
            // if Enemy will soon run out of Stamina, Defend to regain Stamina
            if (EnemyStamina <= 1) return Stance.Defend;

            int stance = Functions.GenerateRandomNumber(1, 100);

            if (Decimal.Divide(GameState.CurrentEnemy.CurrentEndurance, GameState.CurrentEnemy.MaximumEndurance) > 0.2m)
            {
                if (stance <= 20)
                    return Stance.Normal;
                if (stance <= 40)
                    return Stance.Berserk;
                if (stance <= 60)
                    return Stance.Parry;
                if (stance <= 80)
                    return Stance.Defend;
                return Stance.Lunge;
            }

            // if Enemy is at less than 20% health, give a 20% chance to attempt to run away.
            if (stance <= 16)
                return Stance.Normal;
            if (stance <= 32)
                return Stance.Berserk;
            if (stance <= 48)
                return Stance.Parry;
            if (stance <= 64)
                return Stance.Defend;
            if (stance <= 80)
                return Stance.Lunge;
            return Stance.Flee;
        }

        /// <summary>The Enemy's turn.</summary>
        private void EnemyTurn()
        {
            if (_enemyStance != Stance.Defend && _enemyStance != Stance.Flee)
                EnemyAttack();
            else if (_enemyStance == Stance.Flee)
            {
                if (EnemyFlee())
                {
                    Functions.AddTextToTextBox(TxtBattle, "The " + GameState.CurrentEnemy.Name + " fled the battle.");
                    EndBattle();
                }
            }
            else if (_enemyStance == Stance.Defend)
                EnemyStamina++;
            if (EnemyStamina > 20)
                EnemyStamina = 20;
        }

        /// <summary>The Enemy attacks.</summary>
        private void EnemyAttack()
        {
            int playerDamage = GameState.CurrentUser.SelectedWeapon.Damage;
            int playerDefense = GameState.CurrentUser.Armor.Defense;
            int enemyDamage = GameState.CurrentEnemy.Weapon.Damage;
            int enemyDefense = GameState.CurrentEnemy.Armor.Defense;

            switch (_playerStance)
            {
                case Stance.Defend:
                    playerDefense *= 2;
                    break;

                case Stance.Berserk:
                    playerDamage *= 2;
                    break;

                case Stance.Lunge:
                    playerDamage = Int32Helper.Parse(playerDamage * 1.5m);
                    playerDefense = Int32Helper.Parse(playerDefense * 0.5m);
                    break;
            }

            switch (_enemyStance)
            {
                case Stance.Normal:
                    EnemyStamina--;
                    break;

                case Stance.Lunge:
                    enemyDamage = Int32Helper.Parse(enemyDamage * 1.5m);
                    enemyDefense = Int32Helper.Parse(enemyDefense * 0.5m);
                    EnemyStamina--;
                    break;

                case Stance.Berserk:
                    enemyDamage *= 2;
                    EnemyStamina -= 2;
                    break;
            }

            int toHit = Functions.GenerateRandomNumber(10, GameState.CurrentEnemy.WeaponSkill);
            int actualHit = Functions.GenerateRandomNumber(10, 90);

            if (toHit >= actualHit) // then hit
            {
                if (_playerStance == Stance.Parry)
                {
                    int parry = Functions.GenerateRandomNumber(10, GameState.CurrentUser.SelectedWeaponSkill);
                    int parryDefend = Functions.GenerateRandomNumber(10, GameState.CurrentEnemy.WeaponSkill);

                    if (parry >= parryDefend) //enemy successfully parries
                    {
                        int actualDamage = Functions.GenerateRandomNumber(10, playerDamage);
                        int actualDefend = Functions.GenerateRandomNumber(10, enemyDefense);
                        string strParry = "You parry the " + GameState.CurrentEnemy.Name + "'s attack and you attack for " + actualDamage + " damage. ";
                        if (actualDamage > actualDefend) //player actually takes damage
                            Functions.AddTextToTextBox(TxtBattle, strParry + "Their armor absorbs " + actualDefend + " damage. " + GameState.CurrentEnemy.TakeDamage(actualDamage - actualDefend));
                        else
                            Functions.AddTextToTextBox(TxtBattle, strParry + "Their armor absorbs all the damage.");
                    }
                    else
                        EnemyInflictsDamage(enemyDamage, playerDefense);
                }
                else
                {
                    int playerBlocks = Functions.GenerateRandomNumber(10, GameState.CurrentUser.Blocking);
                    if (playerBlocks >= GameState.CurrentUser.Blocking)
                        Functions.AddTextToTextBox(TxtBattle, "You block the " + GameState.CurrentEnemy.Name + "'s attack.");
                    else
                        EnemyInflictsDamage(enemyDamage, playerDefense);
                }
            }
            else
                Functions.AddTextToTextBox(TxtBattle, "The " + GameState.CurrentEnemy.Name + " misses.");
        }

        /// <summary>Enemy inflicts damage on Player.</summary>
        /// <param name="enemyDamage">Maximum damage the Enemy can inflict</param>
        /// <param name="playerDefense">Maximum damage the Player can defend against</param>
        private void EnemyInflictsDamage(int enemyDamage, int playerDefense)
        {
            int actualDamage = Functions.GenerateRandomNumber(1, enemyDamage);
            int actualDefend = Functions.GenerateRandomNumber(1, playerDefense);
            string attack = "The " + GameState.CurrentEnemy.Name + " attacks you for " + actualDamage + ". ";
            if (actualDamage > actualDefend)
                Functions.AddTextToTextBox(TxtBattle, attack + "Your armor absorbs " + actualDefend + " damage. " + GameState.CurrentUser.TakeDamage(actualDamage - actualDefend));
            else
                Functions.AddTextToTextBox(TxtBattle, attack + "Your armor absorbs all of it.");
        }

        #endregion Battle Management

        #region Flee Attempts

        /// <summary>The User attempts to flee.</summary>
        /// <returns>Whether the User successfully fled</returns>
        private bool PlayerFlee() => Functions.GenerateRandomNumber(1, 100) <= GameState.CurrentUser.Slipping;

        /// <summary>The Enemy attempts to flee.</summary>
        /// <returns>Whether the Enemy successfully fled</returns>
        private bool EnemyFlee() => Functions.GenerateRandomNumber(1, 100) <= GameState.CurrentEnemy.Slipping;

        #endregion Flee Attempts

        #region Battle Results

        /// <summary>Ends the battle.</summary>
        private void EndBattle()
        {
            DisableButtons();
            BtnInventory.IsEnabled = false;
            BtnDefend.IsEnabled = false;
            _battleOver = true;
            BtnExit.IsEnabled = true;
        }

        /// <summary>The User loses the battle.</summary>
        private void LoseBattle()
        {
            Functions.AddTextToTextBox(TxtBattle, "You have been slain by your opponent!");
            GameState.CurrentUser.Alive = false;

            EndBattle();
        }

        /// <summary>The User wins the battle.</summary>
        private void WinBattle()
        {
            Functions.AddTextToTextBox(TxtBattle, "You have slain your opponent!");

            if (GameState.CurrentUser.Experience < 100)
            {
                int experience = GameState.CurrentEnemy.Level + 1 - GameState.CurrentUser.Level;
                if (experience > 0)
                {
                    Functions.AddTextToTextBox(TxtBattle, GameState.CurrentUser.GainExperience(experience));
                }
            }

            GameState.CurrentUser.SkillPoints++;
            Functions.AddTextToTextBox(TxtBattle, "You have earned a skill point from this battle.");

            GameState.CurrentUser.GoldOnHand += GameState.CurrentEnemy.GoldOnHand;
            Functions.AddTextToTextBox(TxtBattle, "You frisk your opponent's body and find " + GameState.CurrentEnemy.GoldOnHand + " gold.");

            bool blnTakeWeapon = false;

            switch (GameState.CurrentEnemy.Weapon.Type)
            {
                case WeaponType.Light:
                    if (GameState.CurrentUser.LightWeapon.Value < GameState.CurrentEnemy.Weapon.Value)
                    {
                        GameState.CurrentUser.LightWeapon = new Weapon(GameState.CurrentEnemy.Weapon);
                        blnTakeWeapon = true;
                    }
                    break;

                case WeaponType.Heavy:
                    if (GameState.CurrentUser.HeavyWeapon.Value < GameState.CurrentEnemy.Weapon.Value)
                    {
                        GameState.CurrentUser.HeavyWeapon = new Weapon(GameState.CurrentEnemy.Weapon);
                        blnTakeWeapon = true;
                    }
                    break;

                case WeaponType.TwoHanded:
                    if (GameState.CurrentUser.TwoHandedWeapon.Value < GameState.CurrentEnemy.Weapon.Value)
                    {
                        GameState.CurrentUser.TwoHandedWeapon = new Weapon(GameState.CurrentEnemy.Weapon);
                        blnTakeWeapon = true;
                    }
                    break;
            }
            if (!blnTakeWeapon)
                Functions.AddTextToTextBox(TxtBattle, $"You take the {GameState.CurrentEnemy.Name}'s {GameState.CurrentEnemy.Weapon.Name} off their corpse and bring it to the Weapon shop and sell it for {GameState.CurrentEnemy.Weapon.Value / 2} gold.");
            else
                Functions.AddTextToTextBox(TxtBattle, $"You take the {GameState.CurrentEnemy.Name}'s {GameState.CurrentEnemy.Weapon.Name} off their corpse.");

            EndBattle();
        }

        #endregion Battle Results

        #region Button-Click Methods

        private void BtnAttack_Click(object sender, RoutedEventArgs e)
        {
            _playerStance = Stance.Normal;
            NewRound();
        }

        private void BtnBerserk_Click(object sender, RoutedEventArgs e)
        {
            _playerStance = Stance.Berserk;
            NewRound();
        }

        private void BtnDefend_Click(object sender, RoutedEventArgs e)
        {
            _playerStance = Stance.Defend;
            NewRound();
        }

        private void BtnFlee_Click(object sender, RoutedEventArgs e)
        {
            _playerStance = Stance.Flee;
            NewRound();
        }

        private void BtnLunge_Click(object sender, RoutedEventArgs e)
        {
            _playerStance = Stance.Lunge;
            NewRound();
        }

        private void BtnParry_Click(object sender, RoutedEventArgs e)
        {
            _playerStance = Stance.Parry;
            NewRound();
        }

        private void BtnQuickCombat_Click(object sender, RoutedEventArgs e) => GameState.DisplayNotification("Quick Combat still under construction.", "Assassin");

        private void BtnExit_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private async void ClosePage()
        {
            if (_battleOver)
            {
                GameState.GoBack();
                await GameState.SaveUser(GameState.CurrentUser);
            }
        }

        public BattlePage() => InitializeComponent();

        private void BattlePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindLabels();
        }

        #endregion Page-Manipulation Methods
    }
}