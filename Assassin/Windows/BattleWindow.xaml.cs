using Extensions;
using System;
using System.ComponentModel;
using System.Windows;

namespace Assassin
{
    /// <summary>
    /// Interaction logic for BattleWindow.xaml
    /// </summary>
    public partial class BattleWindow : INotifyPropertyChanged
    {
        private readonly string nl = Environment.NewLine;
        private int _playerStamina = 20;
        private int _enemyStamina = 20;
        private Stance playerStance = Stance.Normal;
        private Stance enemyStance = Stance.Normal;
        private bool battleOver;
        private string previousWindow = "";

        private enum Stance { Normal, Defend, Berserk, Flee, Lunge, Parry }

        /// <summary>
        /// Sets the previous Window.
        /// </summary>
        /// <param name="windowName">Previous Window name</param>
        internal void SetPreviousWindow(string windowName)
        {
            previousWindow = windowName;
        }

        #region Display Manipulation

        private void CheckButtons()
        {
            if (PlayerStamina > 0)
                EnableButtons();
            else
                btnDefend.IsEnabled = true;
        }

        /// <summary>
        /// Gets a text status for a stamina value.
        /// </summary>
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

        /// <summary>
        /// Adds text to the txtGame Textbox.
        /// </summary>
        /// <param name="newText">Text to be added</param>
        internal void AddTextTT(string newText)
        {
            txtBattle.Text += nl + nl + newText;
            txtBattle.Focus();
            txtBattle.CaretIndex = txtBattle.Text.Length;
            txtBattle.ScrollToEnd();
        }

        #endregion Display Manipulation

        #region Properties

        internal AssassinationWindow RefToAssassinationWindow { get; set; }
        internal JobsWindow RefToJobsWindow { get; set; }

        public int PlayerStamina
        {
            get { return _playerStamina; }
            set { _playerStamina = value; OnPropertyChanged("PlayerStaminaToString"); }
        }

        public int EnemyStamina
        {
            get { return _enemyStamina; }
            set { _enemyStamina = value; OnPropertyChanged("EnemyStaminaToString"); }
        }

        public string PlayerStaminaToString => GetStaminaText(PlayerStamina);

        public string EnemyStaminaToString => GetStaminaText(EnemyStamina);

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Binds information to labels.
        /// </summary>
        private void BindLabels()
        {
            lblEnemyArmor.DataContext = GameState.CurrentEnemy;
            lblEnemyEndurance.DataContext = GameState.CurrentEnemy;
            lblEnemyName.DataContext = GameState.CurrentEnemy;
            lblEnemyStatus.DataContext = this;
            lblEnemyWeapon.DataContext = GameState.CurrentEnemy;
            lblUserArmor.DataContext = GameState.CurrentUser;
            lblUserEndurance.DataContext = GameState.CurrentUser;
            lblUsername.DataContext = GameState.CurrentUser;
            lblUserStatus.DataContext = this;
            lblUserWeapon.DataContext = GameState.CurrentUser;

            Surprise();
        }

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Button Manipulation

        /// <summary>
        /// Disable all the buttons.
        /// </summary>
        private void DisableButtons()
        {
            btnAttack.IsEnabled = false;
            btnBerserk.IsEnabled = false;
            btnFlee.IsEnabled = false;
            btnLunge.IsEnabled = false;
            btnParry.IsEnabled = false;
            btnQuickCombat.IsEnabled = false;
        }

        /// <summary>
        /// Enable all the buttons.
        /// </summary>
        private void EnableButtons()
        {
            btnAttack.IsEnabled = true;
            btnFlee.IsEnabled = true;
            btnLunge.IsEnabled = true;
            btnParry.IsEnabled = true;
            btnDefend.IsEnabled = true;
            btnQuickCombat.IsEnabled = true;
            btnBerserk.IsEnabled = PlayerStamina >= 2;
        }

        #endregion Button Manipulation

        #region Battle Management

        private void Surprise()
        {
            int surprise = GameState.GenerateRandomNumber(1, 100);
            if (surprise <= GameState.CurrentUser.Stealth)
            {
                AddTextTT("You surprise your opponent!");
                PlayerInflictsDamage(GameState.CurrentUser.SelectedWeapon.Damage, GameState.CurrentEnemy.Armor.Defense);
            }
        }

        /// <summary>
        /// A new round of battle.
        /// </summary>
        private void NewRound()
        {
            int playerFirst = GameState.GenerateRandomNumber(1, 100);
            int enemyFirst = GameState.GenerateRandomNumber(1, 100);
            enemyStance = EnemyStance();

            DisableButtons();
            btnDefend.IsEnabled = false;

            if (playerFirst >= enemyFirst) //if player goes first
            {
                PlayerTurn();

                if (!battleOver)
                    EnemyTurn();
            }
            else
            {
                EnemyTurn();

                if (!battleOver)
                    PlayerTurn();
            }
            CheckButtons();
            if (GameState.CurrentEnemy.CurrentEndurance <= 0)
                WinBattle();
            else if (GameState.CurrentUser.CurrentEndurance <= 0)
                LoseBattle();
        }

        /// <summary>
        /// The Player's turn.
        /// </summary>
        private void PlayerTurn()
        {
            if (playerStance != Stance.Defend && playerStance != Stance.Flee)
                PlayerAttack();
            else if (playerStance == Stance.Flee)
            {
                if (PlayerFlee())
                {
                    AddTextTT("You successfully fled the battle.");
                    EndBattle();
                }
            }
            else if (playerStance == Stance.Defend)
                PlayerStamina += 1;
        }

        /// <summary>
        /// The Player attacks.
        /// </summary>
        private void PlayerAttack()
        {
            int playerDamage = GameState.CurrentUser.SelectedWeapon.Damage;
            int playerDefense = GameState.CurrentUser.Armor.Defense;
            int enemyDamage = GameState.CurrentEnemy.Weapon.Damage;
            int enemyDefense = GameState.CurrentEnemy.Armor.Defense;

            switch (enemyStance)
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

            switch (playerStance)
            {
                case Stance.Normal:
                    PlayerStamina -= 1;
                    break;

                case Stance.Berserk:
                    playerDamage *= 2;
                    PlayerStamina -= 2;
                    break;

                case Stance.Lunge:
                    playerDamage = Int32Helper.Parse(playerDamage * 1.5m);
                    playerDefense = Int32Helper.Parse(playerDefense * 0.5m);
                    PlayerStamina -= 1;
                    break;
            }

            int toHit = GameState.GenerateRandomNumber(10, GameState.CurrentUser.SelectedWeaponSkill);
            int actualHit = GameState.GenerateRandomNumber(10, 90);

            if (toHit >= actualHit) // then hit
            {
                if (enemyStance == Stance.Parry)
                {
                    int parry = GameState.GenerateRandomNumber(10, GameState.CurrentEnemy.WeaponSkill);
                    int parryDefend = GameState.GenerateRandomNumber(10, GameState.CurrentUser.SelectedWeaponSkill);

                    if (parry >= parryDefend) //enemy successfully parries
                    {
                        int actualDamage = GameState.GenerateRandomNumber(1, enemyDamage);
                        int actualDefend = GameState.GenerateRandomNumber(1, playerDefense);
                        string strParry = "The " + GameState.CurrentEnemy.Name + " parries your attack and attacks you for " + actualDamage + " damage. ";
                        if (actualDamage > actualDefend) //player actually takes damage
                            AddTextTT(strParry + "Your armor absorbs " + actualDefend + " damage. " + GameState.CurrentUser.TakeDamage(actualDamage - actualDefend));
                        else
                            AddTextTT(strParry + "Your armor absorbs all the damage.");
                    }
                    else
                        PlayerInflictsDamage(playerDamage, enemyDefense);
                }
                else
                {
                    int enemyBlocks = GameState.GenerateRandomNumber(10, GameState.CurrentEnemy.Blocking);
                    if (enemyBlocks >= GameState.CurrentEnemy.Blocking)
                        AddTextTT("The " + GameState.CurrentEnemy.Name + " blocks your attack.");
                    else
                        PlayerInflictsDamage(playerDamage, enemyDefense);
                }
            }
            else
                AddTextTT("You miss.");
        }

        /// <summary>
        /// Player inflicts damage on Enemy.
        /// </summary>
        /// <param name="playerDamage">Maximum damage the player can inflict</param>
        /// <param name="enemyDefense">Maximum damage the enemy can defend against</param>
        private void PlayerInflictsDamage(int playerDamage, int enemyDefense)
        {
            int actualDamage = GameState.GenerateRandomNumber(1, playerDamage);
            int actualDefend = GameState.GenerateRandomNumber(1, enemyDefense);
            string attack = "You attack the " + GameState.CurrentEnemy.Name + " for " + actualDamage + ". ";
            if (actualDamage > actualDefend)
                AddTextTT(attack + "Their armor absorbs " + actualDefend + " damage. " + GameState.CurrentEnemy.TakeDamage(actualDamage - actualDefend));
            else
                AddTextTT(attack + "Their armor absorbs all of it.");
        }

        /// <summary>
        /// Sets the Enemy's stance.
        /// </summary>
        /// <returns>Returns stance</returns>
        private Stance EnemyStance()
        {
            int stance = GameState.GenerateRandomNumber(1, 100);

            if (Decimal.Divide(GameState.CurrentEnemy.CurrentEndurance, GameState.CurrentEnemy.MaximumEndurance) > 0.2m)
            {
                if (stance <= 20)
                    return Stance.Normal;
                else if (stance <= 40)
                    return Stance.Berserk;
                else if (stance <= 60)
                    return Stance.Parry;
                else if (stance <= 80)
                    return Stance.Defend;
                else
                    return Stance.Lunge;
            }
            else
            {
                if (stance <= 10)
                    return Stance.Normal;
                else if (stance <= 20)
                    return Stance.Berserk;
                else if (stance <= 30)
                    return Stance.Parry;
                else if (stance <= 40)
                    return Stance.Defend;
                else if (stance <= 50)
                    return Stance.Lunge;
                else
                    return Stance.Flee;
            }
        }

        /// <summary>
        /// The Enemy's turn.
        /// </summary>
        private void EnemyTurn()
        {
            if (enemyStance != Stance.Defend && enemyStance != Stance.Flee)
                EnemyAttack();
            else if (enemyStance == Stance.Flee)
            {
                if (EnemyFlee())
                {
                    AddTextTT("The " + GameState.CurrentEnemy.Name + " fled the battle.");
                    EndBattle();
                }
            }
            else if (enemyStance == Stance.Defend)
                EnemyStamina += 1;
        }

        /// <summary>
        /// The Enemy attacks.
        /// </summary>
        private void EnemyAttack()
        {
            int playerDamage = GameState.CurrentUser.SelectedWeapon.Damage;
            int playerDefense = GameState.CurrentUser.Armor.Defense;
            int enemyDamage = GameState.CurrentEnemy.Weapon.Damage;
            int enemyDefense = GameState.CurrentEnemy.Armor.Defense;

            switch (playerStance)
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

            switch (enemyStance)
            {
                case Stance.Normal:
                    EnemyStamina -= 1;
                    break;

                case Stance.Lunge:
                    enemyDamage = Int32Helper.Parse(enemyDamage * 1.5m);
                    enemyDefense = Int32Helper.Parse(enemyDefense * 0.5m);
                    EnemyStamina -= 1;
                    break;

                case Stance.Berserk:
                    enemyDamage *= 2;
                    EnemyStamina -= 2;
                    break;
            }

            int toHit = GameState.GenerateRandomNumber(10, GameState.CurrentEnemy.WeaponSkill);
            int actualHit = GameState.GenerateRandomNumber(10, 90);

            if (toHit >= actualHit) // then hit
            {
                if (playerStance == Stance.Parry)
                {
                    int parry = GameState.GenerateRandomNumber(10, GameState.CurrentUser.SelectedWeaponSkill);
                    int parryDefend = GameState.GenerateRandomNumber(10, GameState.CurrentEnemy.WeaponSkill);

                    if (parry >= parryDefend) //enemy successfully parries
                    {
                        int actualDamage = GameState.GenerateRandomNumber(10, playerDamage);
                        int actualDefend = GameState.GenerateRandomNumber(10, enemyDefense);
                        string strParry = "You parry the " + GameState.CurrentEnemy.Name + "'s attack and you attack for " + actualDamage + " damage. ";
                        if (actualDamage > actualDefend) //player actually takes damage
                            AddTextTT(strParry + "Their armor absorbs " + actualDefend + " damage. " + GameState.CurrentEnemy.TakeDamage(actualDamage - actualDefend));
                        else
                            AddTextTT(strParry + "Their armor absorbs all the damage.");
                    }
                    else
                        EnemyInflictsDamage(enemyDamage, playerDefense);
                }
                else
                {
                    int playerBlocks = GameState.GenerateRandomNumber(10, GameState.CurrentUser.Blocking);
                    if (playerBlocks >= GameState.CurrentUser.Blocking)
                        AddTextTT("You block the " + GameState.CurrentEnemy.Name + "'s attack.");
                    else
                        EnemyInflictsDamage(enemyDamage, playerDefense);
                }
            }
            else
                AddTextTT("The " + GameState.CurrentEnemy.Name + " misses.");
        }

        /// <summary>
        /// Enemy inflicts damage on Player.
        /// </summary>
        /// <param name="enemyDamage">Maximum damage the Enemy can inflict</param>
        /// <param name="playerDefense">Maximum damage the Player can defend against</param>
        private void EnemyInflictsDamage(int enemyDamage, int playerDefense)
        {
            int actualDamage = GameState.GenerateRandomNumber(1, enemyDamage);
            int actualDefend = GameState.GenerateRandomNumber(1, playerDefense);
            string attack = "The " + GameState.CurrentEnemy.Name + " attacks you for " + actualDamage + ". ";
            if (actualDamage > actualDefend)
                AddTextTT(attack + "Your armor absorbs " + actualDefend + " damage. " + GameState.CurrentUser.TakeDamage(actualDamage - actualDefend));
            else
                AddTextTT(attack + "Your armor absorbs all of it.");
        }

        #endregion Battle Management

        #region Flee Attempts

        /// <summary>
        /// The User attempts to flee.
        /// </summary>
        /// <returns>Whether the User successfully fled</returns>
        private bool PlayerFlee()
        {
            return (GameState.GenerateRandomNumber(1, 100) <= GameState.CurrentUser.Slipping);
        }

        /// <summary>
        /// The Enemy attempts to flee.
        /// </summary>
        /// <returns>Whether the Enemy successfully fled</returns>
        private bool EnemyFlee()
        {
            return (GameState.GenerateRandomNumber(1, 100) <= GameState.CurrentEnemy.Slipping);
        }

        #endregion Flee Attempts

        #region Battle Results

        /// <summary>
        /// Ends the battle.
        /// </summary>
        private void EndBattle()
        {
            DisableButtons();
            btnInventory.IsEnabled = false;
            btnDefend.IsEnabled = false;
            battleOver = true;
            btnExit.IsEnabled = true;
        }

        /// <summary>
        /// The User loses the battle.
        /// </summary>
        private void LoseBattle()
        {
            AddTextTT("You have been slain by your opponent!");
            GameState.CurrentUser.Alive = false;

            EndBattle();
        }

        /// <summary>
        /// The User wins the battle.
        /// </summary>
        private void WinBattle()
        {
            AddTextTT("You have slain your opponent!");

            if (GameState.CurrentUser.Experience < 100)
            {
                int experience = ((GameState.CurrentEnemy.Level + 1) - GameState.CurrentUser.Level);
                if (experience > 0)
                {
                    AddTextTT(GameState.CurrentUser.GainExperience(experience));
                }
            }

            GameState.CurrentUser.SkillPoints += 1;
            AddTextTT("You have earned a skill point from this battle.");

            GameState.CurrentUser.GoldOnHand += GameState.CurrentEnemy.GoldOnHand;
            AddTextTT("You frisk your opponent's body and find " + GameState.CurrentEnemy.GoldOnHand + " gold.");

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
                AddTextTT("You take the " + GameState.CurrentEnemy.Name + "'s " + GameState.CurrentEnemy.Weapon.Name + " off their corpse and bring it to the Weapon shop and sell it for " + (GameState.CurrentEnemy.Weapon.Value / 2) + " gold.");
            else
                AddTextTT("You take the " + GameState.CurrentEnemy.Name + "'s " + GameState.CurrentEnemy.Weapon.Name + " off their corpse.");

            EndBattle();
        }

        #endregion Battle Results

        #region Button-Click Methods

        private void btnAttack_Click(object sender, RoutedEventArgs e)
        {
            playerStance = Stance.Normal;
            NewRound();
        }

        private void btnBerserk_Click(object sender, RoutedEventArgs e)
        {
            playerStance = Stance.Berserk;
            NewRound();
        }

        private void btnDefend_Click(object sender, RoutedEventArgs e)
        {
            playerStance = Stance.Defend;
            NewRound();
        }

        private void btnFlee_Click(object sender, RoutedEventArgs e)
        {
            playerStance = Stance.Flee;
            NewRound();
        }

        private void btnLunge_Click(object sender, RoutedEventArgs e)
        {
            playerStance = Stance.Lunge;
            NewRound();
        }

        private void btnParry_Click(object sender, RoutedEventArgs e)
        {
            playerStance = Stance.Parry;
            NewRound();
        }

        private void btnQuickCombat_Click(object sender, RoutedEventArgs e)
        {
            new Notification("Quick Combat still under construction.", "Assassin", NotificationButtons.OK, this).ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>
        /// Closes the Window.
        /// </summary>
        private void CloseWindow()
        {
            this.Close();
        }

        public BattleWindow()
        {
            InitializeComponent();
            BindLabels();
        }

        private async void windowBattle_Closing(object sender, CancelEventArgs e)
        {
            if (!battleOver)
            {
                e.Cancel = true;
            }
            else
            {
                await GameState.SaveUser(GameState.CurrentUser);
                switch (previousWindow)
                {
                    case "Job":
                        RefToJobsWindow.Show();
                        break;

                    case "Assassinate":
                        RefToAssassinationWindow.Show();
                        break;
                }
            }
        }

        #endregion Window-Manipulation Methods
    }
}