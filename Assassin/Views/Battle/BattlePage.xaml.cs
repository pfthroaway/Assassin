using Assassin.Models;
using Assassin.Models.Entities;
using Assassin.Models.Enums;
using Assassin.Models.Items;
using Assassin.Views.City;
using Assassin.Views.Guilds;
using Assassin.Views.Player;
using Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Assassin.Views.Battle
{
    /// <summary>Interaction logic for BattlePage.xaml</summary>
    public partial class BattlePage : INotifyPropertyChanged
    {
        private bool _blnDone, _blnWin, _blnSurprise = true, _blnPlayer, _blnGuild;
        private static readonly int _maxStamina = 100;
        private int _playerStamina = _maxStamina;
        private int _enemyStamina = _maxStamina;
        private Stance _playerStance, _enemyStance;
        private int _playerBlocking, _enemyBlocking, _playerDamage, _enemyDamage, _playerWeaponSkill, _enemyWeaponSkill;
        private readonly SolidColorBrush defaultBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#CCCCCC");

        //TODO Consider redoing how death is handled. Should they have to re-login and wait a time based on their level? If I do the Graveyard, that's almost definitely how I should do it.
        //TODO In the original game, it was possible to knock out/be knocked out by your opponent. Should I re-implement that?

        #region Properties

        internal bool BlnJob { get; set; }
        internal AssassinationPage RefToAssassinationPage { get; set; }
        internal GuildPage RefToGuildPage { get; set; }
        internal InnPage RefToInnPage { get; set; }
        internal JobsPage RefToJobsPage { get; set; }

        public int PlayerStamina
        {
            get => _playerStamina;
            set { _playerStamina = value; NotifyPropertyChanged(nameof(PlayerStaminaToString)); }
        }

        public int EnemyStamina
        {
            get => _enemyStamina;
            set { _enemyStamina = value; NotifyPropertyChanged(nameof(EnemyStaminaToString)); }
        }

        public string PlayerStaminaToString => GetStaminaText(PlayerStamina);

        public string EnemyStaminaToString => GetStaminaText(EnemyStamina);

        #endregion Properties

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

        /// <summary>Binds information to labels.</summary>
        private void BindLabels()
        {
            GrpUser.DataContext = GameState.CurrentUser;
            LblPlayerStatus.DataContext = this;
            GrpEnemy.DataContext = GameState.CurrentEnemy;
            LblEnemyStatus.DataContext = this;

            if (_blnSurprise)
                Surprise();
        }

        #endregion Data-Binding

        #region Display Manipulation

        /// <summary>Add text to the TextBox.</summary>
        /// <param name="newText">Text to be added</param>
        private void AddText(string newText) => Functions.AddTextToTextBox(TxtBattle, newText);

        /// <summary>Checks which buttons to be enabled.</summary>
        private void CheckButtons()
        {
            if (PlayerStamina > 0)
                ToggleButtons(true);
            else
                BtnDefend.IsEnabled = true;
        }

        /// <summary>Sets the foreground color of some TextBlocks based on <see cref="User"/>'s and <see cref="Enemy"/>'s stats.</summary>
        private void Display()
        {
            LblPlayerEndurance.Foreground = GameState.CurrentUser.EnduranceRatio <= 0.2m ? Brushes.Red : defaultBrush;
            LblPlayerStatus.Foreground = _playerStamina < 3 ? Brushes.Red : defaultBrush;
            LblEnemyEndurance.Foreground = GameState.CurrentEnemy.EnduranceRatio <= 0.2m ? Brushes.Red : defaultBrush;
            LblEnemyStatus.Foreground = _enemyStamina < 3 ? Brushes.Red : defaultBrush;
            ToggleButtons(_playerStamina > 0 && !_blnDone);
        }

        /// <summary>Gets a text status for a stamina value.</summary>
        /// <param name="stamina">Stamina amount</param>
        /// <returns>Text based on stamina</returns>
        private string GetStaminaText(int stamina)
        {
            if (stamina > 18)
                return "Vigorous";
            else if (stamina > 16)
                return "Robust";
            else if (stamina > 14)
                return "Stalwart";
            else if (stamina > 12)
                return "Beat";
            else if (stamina > 10)
                return "Shaky";
            else if (stamina > 8)
                return "Spent";
            else if (stamina > 6)
                return "Bushed";
            else if (stamina > 4)
                return "Weary";
            else if (stamina > 2)
                return "Tired";
            else
                return "Exhausted";
        }

        #endregion Display Manipulation

        #region Battle Management

        /// <summary>Adjusts the stamina for a given <see cref="Stance"/>.</summary>
        /// <param name="stance">Given <see cref="Stance"/></param>
        /// <param name="stamina">Reference to current stamina value</param>
        private void AdjustStamina(Stance stance, ref int stamina)
        {
            switch (stance)
            {
                case Stance.Defend:
                    {
                        if (stamina < _maxStamina)
                            stamina++;
                        break;
                    }

                case Stance.Berserk:
                    {
                        stamina -= 2;
                        break;
                    }

                default:
                    {
                        stamina--;
                        break;
                    }
            }
        }

        /// <summary>Gives a <see cref="User"/> a bonus.</summary>
        /// <returns>Bonus amount</returns>
        private int Bonus() => GameState.CurrentUser.Level <= 4 ? 11 - GameState.CurrentUser.Level * 5 : 0;

        /// <summary>Handle's an <see cref="Enemy"/>'s attack.</summary>
        private void EnemyAttack()
        {
            if (_playerStance != Stance.Parry)
                EnemyHitsPlayer();
            else if (SkillCheck(GameState.CurrentUser.CurrentWeaponSkill + Bonus()))
            {
                AddText("You parry your opponent's attack!");
                PlayerHitsEnemy();
            }
            else
                EnemyHitsPlayer();
        }

        /// <summary>The <see cref="Enemy"/> hits the <see cref="User"/>.</summary>
        private void EnemyHitsPlayer()
        {
            int eneDamage = Functions.GenerateRandomNumber(_enemyDamage / 2, _enemyDamage);
            int plrDefend = Functions.GenerateRandomNumber(GameState.CurrentUser.Armor.Defense / 2, GameState.CurrentUser.Armor.Defense);
            if (eneDamage > plrDefend)
            {
                int actualDamage = eneDamage - plrDefend;
                AddText($"Your opponent attacks you for {eneDamage} damage, but your armor absorbs {plrDefend} damage. You take {actualDamage} total damage.");
                GameState.CurrentUser.CurrentEndurance -= actualDamage;
            }
            else
                AddText($"Your opponent attacks you for {eneDamage} damage, but your armor absorbs all of it.");
        }

        /// <summary>Chooses an <see cref="Enemy"/>'s <see cref="Stance"/>.</summary>
        private void EnemyStance()
        {
            if (_enemyStamina > 2)
            {
                // if enemy's stamina is above 2, any option
                int type = Functions.GenerateRandomNumber(1, 100);
                switch (type)
                {
                    case object _ when 1 <= type && type <= 20    // 20%
                   :
                        {
                            _enemyStance = Stance.Normal;
                            break;
                        }

                    case object _ when 21 <= type && type <= 40    // 20%
             :
                        {
                            _enemyStance = Stance.Berserk;
                            _enemyDamage *= 2;
                            break;
                        }

                    case object _ when 41 <= type && type <= 60    // 20%
             :
                        {
                            _enemyStance = Stance.Lunge;
                            _enemyDamage *= 2;
                            _enemyWeaponSkill /= 2;
                            break;
                        }

                    case object _ when 61 <= type && type <= 80    // 20%
             :
                        {
                            _enemyStance = Stance.Parry;
                            break;
                        }

                    case object _ when 81 <= type && type <= 100   // 20%
             :
                        {
                            _enemyStance = Stance.Defend;
                            if (_enemyStamina < 20)
                            {
                            }

                            break;
                        }
                }
            }
            else if (_enemyStamina == 1)
            {
                // if enemy's stamina is 1, no berserk option, higher chance of defend
                int type = Functions.GenerateRandomNumber(1, 100);
                switch (type)
                {
                    case object _ when 1 <= type && type <= 20    // 20%
                   :
                        {
                            _enemyStance = Stance.Normal;
                            break;
                        }

                    case object _ when 21 <= type && type <= 40   // 20%
             :
                        {
                            _enemyStance = Stance.Lunge;
                            _enemyDamage *= 2;
                            _enemyWeaponSkill /= 2;
                            break;
                        }

                    case object _ when 41 <= type && type <= 60   // 20%
             :
                        {
                            _enemyStance = Stance.Parry;
                            break;
                        }

                    case object _ when 61 <= type && type <= 100  // 40%
             :
                        {
                            _enemyStance = Stance.Defend;
                            break;
                        }
                }
            }
            else if (_enemyStamina <= 0)
                // if enemy's stamina is 0, only defend
                _enemyStance = Stance.Defend;

            if (_enemyStance == Stance.Defend)
            {
                _enemyBlocking = _enemyBlocking >= 45 ? 90 : _enemyBlocking * 2;
                AddText("Your opponent goes on the defensive and attempts to regain stamina.");
            }
        }

        /// <summary>The <see cref="Enemy"/>'s turn.</summary>
        private void EnemyTurn()
        {
            // If the Enemy is not defending
            // and the Enemy can hit
            // and the Assassin doesn't block
            // then attempt to attack.
            // TODO Re-implement the enemy fleeing if they're below 20% endurance.

            if (_enemyStance != Stance.Defend)
            {
                if (SkillCheck(_enemyWeaponSkill))
                {
                    if (SkillCheck(_playerBlocking + Bonus()) == false)
                        EnemyAttack();
                    else
                        AddText("You block your opponent's attack.");
                }
                else
                    AddText("Your opponent misses you.");
            }
        }

        /// <summary>Sets default values for <see cref="User"/> and <see cref="Enemy"/> blocking, damage, and weapon skill.</summary>
        private void LoadBattle()
        {
            _playerBlocking = GameState.CurrentUser.Blocking;
            _enemyBlocking = GameState.CurrentEnemy.Blocking;
            _playerDamage = GameState.CurrentUser.CurrentWeapon.Damage;
            _enemyDamage = GameState.CurrentEnemy.Weapon.Damage;
            _playerWeaponSkill = GameState.CurrentUser.CurrentWeaponSkill;
            _enemyWeaponSkill = GameState.CurrentEnemy.WeaponSkill;
        }

        /// <summary>Starts a new round of battle.</summary>
        private void NewRound()
        {
            int playerFirst = Functions.GenerateRandomNumber(1, 100);
            int enemyFirst = Functions.GenerateRandomNumber(1, 100);
            EnemyStance();

            ToggleButtons(false);
            BtnDefend.IsEnabled = false;

            if (playerFirst >= enemyFirst)
            {
                PlayerTurn();
                if (GameState.CurrentEnemy.CurrentEndurance > 0)
                    EnemyTurn();
            }
            else
            {
                EnemyTurn();
                if (GameState.CurrentUser.CurrentEndurance > 0)
                    PlayerTurn();
            }

            if (GameState.CurrentEnemy.CurrentEndurance <= 0)
                WinBattle();
            else if (GameState.CurrentUser.CurrentEndurance <= 0)
                LoseBattle();
            else
                RoundReset();

            Display();
        }

        /// <summary>The <see cref="User"/> attacks.</summary>
        private void PlayerAttack()
        {
            if (_enemyStance != Stance.Parry)
                PlayerHitsEnemy();
            else if (SkillCheck(GameState.CurrentEnemy.WeaponSkill))
            {
                AddText("Your opponent parries your attack!");
                EnemyHitsPlayer();
            }
            else
                PlayerHitsEnemy();
        }

        /// <summary>The <see cref="User"/> flees the battle.</summary>
        private void PlayerFlee()
        {
            AddText("You have escaped the battle!");

            if (GameState.CurrentUser.Experience < 100)
            {
                if ((GameState.CurrentEnemy.Level - GameState.CurrentUser.Level) >= 2)
                {
                    AddText("You have gained 1 experience from the battle.");
                    GameState.CurrentUser.Experience++;
                }
            }

            ToggleButtons(false);                    // disable buttons
            BtnExit.IsEnabled = true;              // enable Exit buton
            _blnDone = true;                      // allow the form to exit
        }

        /// <summary>The <see cref="User"/> hits the <see cref="Enemy"/>.</summary>
        private void PlayerHitsEnemy()
        {
            int plrDamage = Functions.GenerateRandomNumber(_playerDamage / 2, _playerDamage);
            int eneDefend = Functions.GenerateRandomNumber(GameState.CurrentEnemy.Armor.Defense / 2, GameState.CurrentEnemy.Armor.Defense);

            if (plrDamage > eneDefend)
            {
                int actualDamage = plrDamage - eneDefend;
                AddText($"You attack your opponent for {plrDamage} damage, but their armor absorbs {eneDefend} damage. You do {actualDamage} total damage.");
                GameState.CurrentEnemy.CurrentEndurance -= actualDamage;
                if (GameState.CurrentEnemy.CurrentEndurance < 0)
                    GameState.CurrentEnemy.CurrentEndurance = 0;
            }
            else
                AddText($"You attack your opponent for {plrDamage} damage, but their armor absorbs all of it.");
        }

        /// <summary>Handles the <see cref="User"/>'s <see cref="Stance"/> in QuickCombat.</summary>
        private void QuickCombatPlayerStance()
        {
            if (_playerStamina > 2)
            {
                int percent = Functions.GenerateRandomNumber(1, 100);
                if (percent <= 20)
                    SetPlayerStance(Stance.Normal);
                else if (percent <= 40)
                    SetPlayerStance(Stance.Berserk);
                else if (percent <= 60)
                    SetPlayerStance(Stance.Parry);
                else if (percent <= 80)
                    SetPlayerStance(Stance.Lunge);
                else
                    SetPlayerStance(Stance.Defend);
            }
            else if (_playerStamina == 1)
            {
                int percent = Functions.GenerateRandomNumber(1, 100);
                if (percent <= 20)
                    SetPlayerStance(Stance.Normal);
                else if (percent <= 40) SetPlayerStance(Stance.Parry);
                else if (percent <= 60)
                    SetPlayerStance(Stance.Lunge);
                else
                    SetPlayerStance(Stance.Defend);
            }
            else
                SetPlayerStance(Stance.Defend);
        }

        /// <summary>The <see cref="User"/>'s turn.</summary>
        private void PlayerTurn()
        {
            if (_playerStance != Stance.Defend && _playerStance != Stance.Flee)
            {
                if (SkillCheck(_playerWeaponSkill + Bonus()))
                {
                    if (!SkillCheck(_enemyBlocking))
                        PlayerAttack();
                    else
                        AddText("Your opponent blocks your attack.");
                }
                else
                    AddText("You miss your opponent.");
            }
        }

        /// <summary>Resets <see cref="User"/> and <see cref="Enemy"/> stats after a round.</summary>
        private void RoundReset()
        {
            AdjustStamina(_playerStance, ref _playerStamina);
            _playerBlocking = GameState.CurrentUser.Blocking;
            _playerDamage = GameState.CurrentUser.CurrentWeapon.Damage;
            _playerWeaponSkill = GameState.CurrentUser.CurrentWeaponSkill;

            AdjustStamina(_enemyStance, ref _enemyStamina);
            _enemyBlocking = GameState.CurrentEnemy.Blocking;
            _enemyDamage = GameState.CurrentEnemy.Weapon.Damage;
            _enemyWeaponSkill = GameState.CurrentEnemy.WeaponSkill;
        }

        /// <summary>Sets the <see cref="User"/>'s <see cref="Stance"/>.</summary>
        /// <param name="stance"><see cref="Stance"/> to be set</param>
        private void SetPlayerStance(Stance stance)
        {
            _playerStance = stance;

            switch (_playerStance)
            {
                case Stance.Berserk:
                    {
                        _playerDamage *= 2;
                        break;
                    }

                case Stance.Lunge:
                    {
                        _playerDamage *= 2;
                        _playerWeaponSkill /= 2;
                        break;
                    }

                case Stance.Defend:
                    {
                        _playerBlocking = _playerBlocking >= 45 ? 90 : _playerBlocking * 2;
                        break;
                    }
            }
        }

        /// <summary>Sets the <see cref="User"/>'s <see cref="Stance"/> and starts a new round.</summary>
        /// <param name="stance">Stance to be set</param>
        private void SetPlayerStanceNewRound(Stance stance)
        {
            SetPlayerStance(stance);
            NewRound();
        }

        /// <summary>Determines whether an attempt to hit is successful.</summary>
        /// <param name="skill">Skill to be tested against</param>
        /// <returns>True if successful hit</returns>
        private bool SkillCheck(int skill) => Functions.GenerateRandomNumber(1, 100) <= (skill > 90 ? 90 : skill);

        /// <summary>Determines if you surprise the <see cref="Enemy"/> when first attacking them.</summary>
        private void Surprise()
        {
            LoadBattle();
            if (!_blnPlayer)
            {
                AddText($"You approach the {GameState.CurrentEnemy.Name}.");
                if (SkillCheck(GameState.CurrentUser.Stealth + Bonus()))
                    SurpriseSuccess();
            }
            else if (_blnGuild)
            {
                AddText($"You challenge {GameState.CurrentEnemy.Name} to become the master of {GameState.CurrentGuild.Name}.");
            }
            else
            {
                AddText(RefToInnPage != null ? $"You creep into the room where {GameState.CurrentEnemy} is sleeping..." : $"You head into the dark alleyway where {GameState.CurrentEnemy.Name} was last seen sleeping.");
                int awake = Functions.GenerateRandomNumber(10, GameState.CurrentEnemy.Level * 8 + 2);
                if (awake >= GameState.CurrentUser.Stealth + Bonus())
                {
                    AddText($"{GameState.CurrentEnemy} was waiting for you!");
                    if (SkillCheck(GameState.CurrentUser.Stealth + Bonus()))
                        SurpriseSuccess();
                }
                else
                {
                    AddText($"You successfully sneak up on {GameState.CurrentEnemy.Name} while they're sleeping!");
                    SurpriseSuccess();
                }
            }
            _blnSurprise = false;
        }

        private void SurpriseSuccess()
        {
            _playerDamage *= 2;
            AddText("You surprise your opponent!");
            PlayerHitsEnemy();

            if (GameState.CurrentEnemy.CurrentEndurance <= 0)
                WinBattle();
            else
                RoundReset();
        }

        #endregion Battle Management

        #region Battle Results

        /// <summary>Ends the battle.</summary>
        private void EndBattle()
        {
            _blnDone = true;
            GameState.MainWindow.BlnPreventClosing = false;
            ToggleButtons(false);
            BtnExit.IsEnabled = true;
        }

        /// <summary>Saves the <see cref="User"/> who was an <see cref="Enemy"/> during the battle.</summary>
        private async void EndPlayerBattle()
        {
            GameState.AllUsers.Find(user => user.Name == GameState.CurrentEnemy.Name).ConvertFromEnemy(GameState.CurrentEnemy);
            await GameState.DatabaseInteraction.SaveUser(GameState.AllUsers.Find(user => user.Name == GameState.CurrentEnemy.Name));
        }

        /// <summary>Handles losing a <see cref="Guild"/> battle.</summary>
        private async void LoseGuildBattle()
        {
            Functions.AddTextToTextBox(RefToGuildPage.TxtGuild, "You have been defeated by the guildmaster. You also have been expelled for your insolence.");
            RefToGuildPage.DisableButtons();
            await GameState.MemberLeavesGuild(GameState.CurrentUser, GameState.CurrentGuild);
        }

        /// <summary>The Player loses the battle.</summary>
        private void LoseBattle()
        {
            AddText("You have been slain by your opponent!");
            GameState.CurrentUser.CurrentEndurance = 0;
            GameState.CurrentUser.Alive = false;

            EndBattle();
        }

        /// <summary>The <see cref="User"/> wins the battle.</summary>
        private async void WinBattle()
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

            if (GameState.CurrentUser.Level - GameState.CurrentEnemy.Level >= -2)
            {
                GameState.CurrentUser.SkillPoints++;
                Functions.AddTextToTextBox(TxtBattle, "You have earned a skill point from this battle.");
            }

            GameState.CurrentUser.GoldOnHand += GameState.CurrentEnemy.GoldOnHand;
            Functions.AddTextToTextBox(TxtBattle, "You frisk your opponent's body and find " + GameState.CurrentEnemy.GoldOnHand + " gold.");

            bool blnTakeWeapon = false;
            int difference = 0;
            switch (GameState.CurrentEnemy.Weapon.Type)
            {
                case WeaponType.Light:
                    if (GameState.CurrentUser.LightWeapon.Value < GameState.CurrentEnemy.Weapon.Value)
                    {
                        difference = GameState.CurrentEnemy.Weapon.Value - GameState.CurrentUser.LightWeapon.Value;
                        GameState.CurrentUser.LightWeapon = new Weapon(GameState.CurrentEnemy.Weapon);
                        blnTakeWeapon = true;
                    }
                    break;

                case WeaponType.Heavy:
                    if (GameState.CurrentUser.HeavyWeapon.Value < GameState.CurrentEnemy.Weapon.Value)
                    {
                        difference = GameState.CurrentEnemy.Weapon.Value - GameState.CurrentUser.HeavyWeapon.Value;
                        GameState.CurrentUser.HeavyWeapon = new Weapon(GameState.CurrentEnemy.Weapon);
                        blnTakeWeapon = true;
                    }
                    break;

                case WeaponType.TwoHanded:
                    if (GameState.CurrentUser.TwoHandedWeapon.Value < GameState.CurrentEnemy.Weapon.Value)
                    {
                        difference = GameState.CurrentEnemy.Weapon.Value - GameState.CurrentUser.TwoHandedWeapon.Value;
                        GameState.CurrentUser.TwoHandedWeapon = new Weapon(GameState.CurrentEnemy.Weapon);
                        blnTakeWeapon = true;
                    }
                    break;
            }
            if (!blnTakeWeapon)
            {
                Functions.AddTextToTextBox(TxtBattle, $"You take your opponent's {GameState.CurrentEnemy.Weapon.Name} off their corpse and bring it to the Weapon shop and sell it for {GameState.CurrentEnemy.Weapon.SellValue} gold.");
                GameState.CurrentUser.GoldOnHand += GameState.CurrentEnemy.Weapon.SellValue;
            }
            else
            {
                string weaponText = $"You take your opponent's {GameState.CurrentEnemy.Weapon.Name} off their corpse.";
                if (difference > 0)
                {
                    weaponText += $" You take your old weapon to the Weapon shop and sell it for {difference} gold.";
                    GameState.CurrentUser.GoldOnHand += difference;
                }
                Functions.AddTextToTextBox(TxtBattle, weaponText);
            }

            EndBattle();
            _blnWin = true;
            await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
        }

        /// <summary>Handles winning a <see cref="Guild"/> battle.</summary>
        private async void WinGuildBattle()
        {
            Functions.AddTextToTextBox(RefToGuildPage.TxtGuild, "You have defeated the guildmaster! You are now the guild leader.");
            GameState.CurrentGuild.Master = GameState.CurrentUser.Name;
            await GameState.DatabaseInteraction.SaveGuild(GameState.CurrentGuild);
        }

        #endregion Battle Results

        #region Button Manipulation

        /// <summary>Toggles all the battle Buttons.</summary>
        /// <param name="enabled">Should the battle Buttons be enabled?</param>
        private void ToggleButtons(bool enabled)
        {
            BtnAttack.IsEnabled = enabled;
            BtnBerserk.IsEnabled = enabled && PlayerStamina >= 2;
            BtnDefend.IsEnabled = !_blnDone;
            BtnFlee.IsEnabled = enabled;
            BtnInventory.IsEnabled = !_blnDone;
            BtnLunge.IsEnabled = enabled;
            BtnParry.IsEnabled = enabled;
            BtnQuickCombat.IsEnabled = enabled;
        }

        #endregion Button Manipulation

        #region Button-Click Methods

        private void BtnAttack_Click(object sender, RoutedEventArgs e) => SetPlayerStanceNewRound(Stance.Normal);

        private void BtnBerserk_Click(object sender, RoutedEventArgs e) => SetPlayerStanceNewRound(Stance.Berserk);

        private void BtnDefend_Click(object sender, RoutedEventArgs e) => SetPlayerStanceNewRound(Stance.Defend);

        private void BtnFlee_Click(object sender, RoutedEventArgs e)
        {
            if (SkillCheck(GameState.CurrentUser.Slipping + Bonus()))
            {
                if (SkillCheck(_enemyBlocking - Bonus()))
                    PlayerFlee();
                else
                {
                    _playerStance = Stance.Flee;
                    AddText("Your opponent blocked your attempt to flee.");
                    NewRound();
                }
            }
            else
            {
                _playerStance = Stance.Flee;
                AddText("Your attempt at flight failed miserably.");
                NewRound();
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void BtnInventory_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new InventoryPage());

        private void BtnLunge_Click(object sender, RoutedEventArgs e) => SetPlayerStanceNewRound(Stance.Lunge);

        private void BtnParry_Click(object sender, RoutedEventArgs e) => SetPlayerStanceNewRound(Stance.Parry);

        private void BtnQuickCombat_Click(object sender, RoutedEventArgs e)
        {
            while (GameState.CurrentUser.CurrentEndurance > 0 && GameState.CurrentEnemy.CurrentEndurance > 0)
            {
                QuickCombatPlayerStance();
                NewRound();
            }
        }

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private async void ClosePage()
        {
            if (_blnDone)
            {
                if (BlnJob)
                {
                    Functions.AddTextToTextBox(RefToJobsPage.TxtJob, TxtBattle.Text.Trim());
                    RefToJobsPage.BtnLeaveTable.IsEnabled = true;
                    if (_blnWin)
                    {
                        Functions.AddTextToTextBox(RefToJobsPage.TxtJob, "You take your opponent's engraved weapon back to your employer.");
                        RefToJobsPage.GetPaid();
                    }
                }
                else if (_blnPlayer && _blnGuild)
                {
                    if (_blnWin)
                        WinGuildBattle();
                    else
                        LoseGuildBattle();

                    EndPlayerBattle();
                }
                else if (_blnPlayer)
                {
                    GameState.MainWindow.MainFrame.RemoveBackEntry();
                    if (!GameState.CurrentUser.Alive && RefToInnPage != null)
                        RefToInnPage.DisableButtons();
                    Functions.AddTextToTextBox(RefToInnPage != null ? RefToInnPage.TxtInn : GameState.GamePage.TxtGame, TxtBattle.Text.Trim());
                    EndPlayerBattle();
                }
                else if (_blnGuild)
                {
                    Functions.AddTextToTextBox(RefToGuildPage.TxtGuild, TxtBattle.Text.Trim());
                    if (_blnWin)
                        WinGuildBattle();
                    else
                        LoseGuildBattle();
                }
                else
                    Functions.AddTextToTextBox(RefToAssassinationPage.TxtAssassinate, TxtBattle.Text.Trim());

                GameState.GoBack();
                await GameState.DatabaseInteraction.SaveUser(GameState.CurrentUser);
            }
        }

        public BattlePage(bool player = false, bool guild = false)
        {
            InitializeComponent();
            _blnPlayer = player;
            _blnGuild = guild;
            GameState.MainWindow.BlnPreventClosing = true;
            GameState.MainWindow.TxtPreventClosing = "You must finish your battle first.";
        }

        private void BattlePage_OnLoaded(object sender, RoutedEventArgs e) => BindLabels();

        #endregion Page-Manipulation Methods
    }
}