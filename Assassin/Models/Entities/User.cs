using Assassin.Models.Enums;
using Assassin.Models.Items;
using System;

namespace Assassin.Models.Entities
{
    /// <summary>Represents a <see cref="User"/> in the game.</summary>
    public class User : LivingEntity
    {
        //TODO Finish BaseINPC on all classes
        //TODO Check all properties to make sure they're used right.

        private string _password;

        private int _experience, _skillPoints, _hunger, _thirst, _lockpicks, _goldInBank, _goldOnLoan, _lightWeaponSkill, _heavyWeaponSkill, _twoHandedWeaponSkill, _stealth, _henchmenLevel1, _henchmenLevel2, _henchmenLevel3, _henchmenLevel4, _henchmenLevel5;
        private bool _alive, _shovel, _lantern, _amulet;
        private SleepLocation _currentLocation;
        private WeaponType _currentWeapon;
        private Weapon _lightWeapon, _heavyWeapon, _twoHandedWeapon;
        private Potion _potion;

        #region Properties

        /// <summary><see cref="User"/>'s Password.</summary>
        public string Password
        {
            get => _password;
            set { _password = value; NotifyPropertyChanged(nameof(Password)); }
        }

        /// <summary>Rank of User</summary>
        public string Rank => GameState.AllRanks[Level - 1];

        /// <summary><see cref="User"/>'s current experience points.</summary>
        public int Experience
        {
            get => _experience;
            set { _experience = value; NotifyPropertyChanged(nameof(Experience)); }
        }

        /// <summary><see cref="User"/>'s available skill points to spend</summary>
        public int SkillPoints
        {
            get => _skillPoints;
            set { _skillPoints = value; NotifyPropertyChanged(nameof(SkillPoints), nameof(SkillPointsToString)); }
        }

        /// <summary>Is the User alive?</summary>
        public bool Alive
        {
            get => _alive;
            set { _alive = value; NotifyPropertyChanged(nameof(Alive)); }
        }

        /// <summary>Current location of the <see cref="User"/>.</summary>
        public SleepLocation CurrentLocation
        {
            get => _currentLocation;
            set { _currentLocation = value; NotifyPropertyChanged(nameof(CurrentLocation)); }
        }

        /// <summary>Amount of hunger a <see cref="User"/> has.</summary>
        public int Hunger
        {
            get => _hunger;
            set
            {
                _hunger = value;
                NotifyPropertyChanged(nameof(Hunger), nameof(HungerToString));
            }
        }

        /// <summary>Amount of thirst a <see cref="User"/> has.</summary>
        public int Thirst
        {
            get => _thirst;
            set
            {
                _thirst = value;
                NotifyPropertyChanged(nameof(Thirst), nameof(ThirstToString));
            }
        }

        /// <summary>Amount of hunger a <see cref="User"/> has, formatted.</summary>
        public string HungerToString
        {
            get
            {
                if (Hunger < 5)
                    return "Full";
                else if (Hunger < 10)
                    return "Hungry";
                else if (Hunger < 15)
                    return "Very Hungry";
                else if (Hunger < 20)
                    return "Famished";
                else if (Hunger < 25)
                    return "Starving";
                else
                    return "BROKEN";
            }
        }

        /// <summary>Amount of thirst a <see cref="User"/> has, formatted.</summary>
        public string ThirstToString
        {
            get
            {
                if (Thirst < 5)
                    return "Quenched";
                else if (Thirst < 10)
                    return "Thirsty";
                else if (Thirst < 15)
                    return "Very Thirsty";
                else if (Thirst < 20)
                    return "Parched";
                else if (Thirst < 25)
                    return "Dehydrated";
                else
                    return "BROKEN";
            }
        }

        /// <summary>Weapon type of <see cref="User"/>'s currently equipped <see cref="Weapon"/>.</summary>
        public WeaponType CurrentWeaponType
        {
            get => _currentWeapon;
            set { _currentWeapon = value; NotifyPropertyChanged(nameof(CurrentWeaponType)); }
        }

        /// <summary><see cref="User"/>'s light <see cref="Weapon"/>.</summary>
        public Weapon LightWeapon
        {
            get => _lightWeapon;
            set { _lightWeapon = value; NotifyPropertyChanged(nameof(LightWeapon)); }
        }

        /// <summary><see cref="User"/>'s heavy weapon.</summary>
        public Weapon HeavyWeapon
        {
            get => _heavyWeapon;
            set { _heavyWeapon = value; NotifyPropertyChanged(nameof(HeavyWeapon)); }
        }

        /// <summary><see cref="User"/>'s two-handed weapon.</summary>
        public Weapon TwoHandedWeapon
        {
            get => _twoHandedWeapon;
            set { _twoHandedWeapon = value; NotifyPropertyChanged(nameof(TwoHandedWeapon)); }
        }

        /// <summary>User's potion.</summary>
        public Potion Potion
        {
            get => _potion;
            set { _potion = value; NotifyPropertyChanged(nameof(Potion)); }
        }

        /// <summary>Amount of lockpicks a <see cref="User"/> has.</summary>
        public int Lockpicks
        {
            get => _lockpicks;
            set { _lockpicks = value; NotifyPropertyChanged(nameof(Lockpicks), nameof(LockpicksToString), nameof(LockpicksToStringWithText)); }
        }

        /// <summary>Does the <see cref="User"/> have a shovel?</summary>
        public bool Shovel
        {
            get => _shovel;
            set { _shovel = value; NotifyPropertyChanged(nameof(Shovel)); }
        }

        /// <summary>Does the <see cref="User"/> have a lantern?</summary>

        public bool Lantern
        {
            get => _lantern;
            set { _lantern = value; NotifyPropertyChanged(nameof(Lantern)); }
        }

        /// <summary>Does the <see cref="User"/> have an amulet?</summary>
        public bool Amulet
        {
            get => _amulet;
            set { _amulet = value; NotifyPropertyChanged(nameof(Amulet)); }
        }

        /// <summary>Amount of gold the <see cref="User"/> has in the bank.</summary>
        public int GoldInBank
        {
            get => _goldInBank;
            set { _goldInBank = value; NotifyPropertyChanged(nameof(GoldInBank)); }
        }

        /// <summary>Amount of gold the <see cref="User"/> has taken as a loan from the bank.</summary>
        public int GoldOnLoan
        {
            get => _goldOnLoan;
            set { _goldOnLoan = value; NotifyPropertyChanged(nameof(GoldOnLoan), nameof(GoldOnLoanToString), nameof(LoanAvailable), nameof(LoanAvailableToString)); }
        }

        /// <summary><see cref="User"/>'s skill with light weapons.</summary>
        public int LightWeaponSkill
        {
            get => _lightWeaponSkill;
            set { _lightWeaponSkill = value; NotifyPropertyChanged(nameof(LightWeaponSkill)); }
        }

        /// <summary><see cref="User"/>'s skill with heavy weapons.</summary>
        public int HeavyWeaponSkill
        {
            get => _heavyWeaponSkill;
            set { _heavyWeaponSkill = value; NotifyPropertyChanged(nameof(HeavyWeaponSkill)); }
        }

        /// <summary><see cref="User"/>'s skill with two-handed weapons.</summary>
        public int TwoHandedWeaponSkill
        {
            get => _twoHandedWeaponSkill;
            set { _twoHandedWeaponSkill = value; NotifyPropertyChanged(nameof(TwoHandedWeaponSkill)); }
        }

        /// <summary><see cref="User"/>'s skill at being stealthy.</summary>
        public int Stealth
        {
            get => _stealth;
            set { _stealth = value; NotifyPropertyChanged(nameof(Stealth)); }
        }

        /// <summary>Amount of level 1 henchmen hired by the <see cref="User"/>.</summary>
        public int HenchmenLevel1
        {
            get => _henchmenLevel1;
            set { _henchmenLevel1 = value; NotifyPropertyChanged(nameof(HenchmenLevel1)); }
        }

        /// <summary>Amount of level 2 henchmen hired by the <see cref="User"/>.</summary>
        public int HenchmenLevel2
        {
            get => _henchmenLevel2;
            set { _henchmenLevel2 = value; NotifyPropertyChanged(nameof(HenchmenLevel2)); }
        }

        /// <summary>Amount of level 3 henchmen hired by the <see cref="User"/>.</summary>
        public int HenchmenLevel3
        {
            get => _henchmenLevel3;
            set { _henchmenLevel3 = value; NotifyPropertyChanged(nameof(HenchmenLevel3)); }
        }

        /// <summary>Amount of level 4 henchmen hired by the <see cref="User"/>.</summary>
        public int HenchmenLevel4
        {
            get => _henchmenLevel4;
            set { _henchmenLevel4 = value; NotifyPropertyChanged(nameof(HenchmenLevel4)); }
        }

        /// <summary>Amount of level 5 henchmen hired by the <see cref="User"/>.</summary>
        public int HenchmenLevel5
        {
            get => _henchmenLevel5;
            set { _henchmenLevel5 = value; NotifyPropertyChanged(nameof(HenchmenLevel5)); }
        }

        #endregion Properties

        #region Helper Properties

        /// <summary>Amount of gold the <see cref="User"/> has in the bank, formatted.</summary>
        public string GoldInBankToString => GoldInBank.ToString("N0");

        /// <summary>Amount of gold the <see cref="User"/> has taken as a loan from the bank, formatted.</summary>
        public string GoldOnLoanToString => GoldOnLoan.ToString("N0");

        /// <summary>Amount of gold the <see cref="User"/> can take as a loan from the bank.</summary>
        public int LoanAvailable => (Level * 250) - GoldOnLoan;

        /// <summary>Amount of gold the <see cref="User"/> can take as a loan from the bank, formatted.</summary>
        public string LoanAvailableToString => (LoanAvailable - GoldOnLoan).ToString("N0");

        /// <summary>Amount of lockpicks a <see cref="User"/> has, formatted.</summary>
        public string LockpicksToString => Lockpicks.ToString("N0");

        /// <summary>Amount of lockpicks a <see cref="User"/> has, formatted, with preceding text.</summary>
        public string LockpicksToStringWithText => $"Lockpicks on Hand: {LockpicksToString}";

        /// <summary><see cref="User"/>'s skill with their currently equipped weapon.</summary>
        public int SelectedWeaponSkill
        {
            get
            {
                switch (CurrentWeaponType)
                {
                    case WeaponType.Light:
                        return LightWeaponSkill;

                    case WeaponType.Heavy:
                        return HeavyWeaponSkill;

                    case WeaponType.TwoHanded:
                        return TwoHandedWeaponSkill;
                }
                return 0;
            }
        }

        /// <summary><see cref="User"/>'s currently equipped weapon.</summary>
        public Weapon CurrentWeapon
        {
            get
            {
                switch (CurrentWeaponType)
                {
                    case WeaponType.Light:
                        return LightWeapon;

                    case WeaponType.Heavy:
                        return HeavyWeapon;

                    case WeaponType.TwoHanded:
                        return TwoHandedWeapon;
                }
                return new Weapon();
            }
        }

        /// <summary><see cref="User"/>'s available skill points to spend, formatted</summary>
        public string SkillPointsToString => SkillPoints.ToString("N0") + " Skill Points Available";

        #endregion Helper Properties

        /// <summary>Determines if the <see cref="User"/> can do an action.</summary>
        /// <returns>If the <see cref="User"/> can do an action</returns>
        public bool CanDoAction() => Hunger < 24 && Thirst < 24;

        /// <summary>Displays text about the <see cref="User"/>'s current hunger and thirst.</summary>
        /// <returns>text about the <see cref="User"/>'s current hunger and thirst</returns>
        public string DisplayHungerThirstText()
        {
            if (Hunger >= 24 && Thirst >= 24)
                return "You are too hungry and thirsty to continue.";
            else if (Hunger >= 24)
                return "You are too hungry to continue.";
            else if (Thirst >= 24)
                return "You are too thirsty to continue.";
            else if (Hunger > 0 && Hunger % 5 == 0)
                return $"You are {GameState.GetHunger(Hunger).ToLower()}.";
            else if (Thirst > 0 && Thirst % 5 == 0)
                return $"You are {GameState.GetThirst(Thirst).ToLower()}.";

            return "";
        }

        /// <summary>Gains experience for the <see cref="User"/>.</summary>
        /// <param name="experience">Experience gained</param>
        /// <returns>String based on experience gain</returns>
        public string GainExperience(int experience)
        {
            int oldExperience = Experience;
            string experienceText = $"You have earned {experience} experience from the battle.";
            string levelText = "";

            Experience += experience;
            if (oldExperience / 10 < Experience / 10)
            {
                Level++;
                levelText = $" You have gained a level! You are now a {Rank}!";
            }

            //if past maximum exp, set to maximum
            if (Experience > 100)
                Experience = 100;

            return experienceText + levelText;
        }

        /// <summary>Gains hunger and thirst, and returns text regarding their new hunger and thirst states.</summary>
        /// <returns>Text regarding new hunger and thirst states</returns>
        public string GainHungerThirst()
        {
            Hunger++;
            Thirst++;
            return DisplayHungerThirstText();
        }

        #region Health Manipulation

        /// <summary>The <see cref="User"/> heals themselves.</summary>
        /// <param name="healAmount">Amount of health restored.</param>
        /// <returns>Message regarding damage taken</returns>
        public string Heal(int healAmount)
        {
            CurrentEndurance += healAmount;
            if (CurrentEndurance > MaximumEndurance)
                CurrentEndurance = MaximumEndurance;
            return $"You heal {healAmount:N0} damage.";
        }

        /// <summary>The <see cref="User"/> takes damage.</summary>
        /// <param name="damage">Amount of damage taken.</param>
        /// <returns>Message regarding damage taken</returns>
        public override string TakeDamage(int damage)
        {
            CurrentEndurance -= damage;
            return $"You take {damage:N0} damage.";
        }

        #endregion Health Manipulation

        #region Override Operators

        public static bool Equals(User left, User right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase)
                && left.Level == right.Level
                && (left.Experience == right.Experience)
                && (left.SkillPoints == right.SkillPoints)
                && (left.Alive == right.Alive)
                && (left.CurrentLocation == right.CurrentLocation)
                && (left.CurrentEndurance == right.CurrentEndurance)
                && (left.MaximumEndurance == right.MaximumEndurance)
                && (left.Hunger == right.Hunger)
                && (left.Thirst == right.Thirst)
                && (left.CurrentWeaponType == right.CurrentWeaponType)
                && (left.LightWeapon == right.LightWeapon)
                && (left.HeavyWeapon == right.HeavyWeapon)
                && (left.TwoHandedWeapon == right.TwoHandedWeapon)
                && (left.Armor == right.Armor)
                && (left.Potion == right.Potion)
                && (left.Lockpicks == right.Lockpicks)
                && (left.GoldOnHand == right.GoldOnHand)
                && (left.GoldInBank == right.GoldInBank)
                && (left.GoldOnLoan == right.GoldOnLoan)
                && (left.Shovel == right.Shovel)
                && (left.Lantern == right.Lantern)
                && (left.Amulet == right.Amulet)
                && (left.LightWeaponSkill == right.LightWeaponSkill)
                && (left.HeavyWeaponSkill == right.HeavyWeaponSkill)
                && (left.TwoHandedWeaponSkill == right.TwoHandedWeaponSkill)
                && (left.Blocking == right.Blocking)
                && (left.Slipping == right.Slipping)
                && (left.Stealth == right.Stealth)
                && (left.HenchmenLevel1 == right.HenchmenLevel1)
                && (left.HenchmenLevel2 == right.HenchmenLevel2)
                && (left.HenchmenLevel3 == right.HenchmenLevel3)
                && (left.HenchmenLevel4 == right.HenchmenLevel4)
                && (left.HenchmenLevel5 == right.HenchmenLevel5);
        }

        public override bool Equals(object obj) => Equals(this, obj as User);

        public bool Equals(User other) => Equals(this, other);

        public static bool operator ==(User left, User right) => Equals(left, right);

        public static bool operator !=(User left, User right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="User"/> class.</summary>
        public User()
        {
            Name = "";
            Password = "";
            Level = 1;
            Experience = 0;
            SkillPoints = 5;
            Alive = true;
            CurrentLocation = SleepLocation.Streets;
            CurrentEndurance = 100;
            MaximumEndurance = 100;
            Hunger = 0;
            Thirst = 0;

            CurrentWeaponType = WeaponType.Light;
            LightWeapon = new Weapon();
            HeavyWeapon = new Weapon();
            TwoHandedWeapon = new Weapon();
            Armor = new Armor();
            Potion = new Potion();
            Lockpicks = 0;
            GoldOnHand = 100;
            GoldInBank = 0;
            GoldOnLoan = 0;
            Shovel = false;
            Lantern = false;
            Amulet = false;

            LightWeaponSkill = 10;
            HeavyWeaponSkill = 10;
            TwoHandedWeaponSkill = 10;
            Blocking = 10;
            Slipping = 10;
            Stealth = 10;

            HenchmenLevel1 = 0;
            HenchmenLevel2 = 0;
            HenchmenLevel3 = 0;
            HenchmenLevel4 = 0;
            HenchmenLevel5 = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="User"/> class using Property values.</summary>
        /// <param name="name">Name of <see cref="User"/></param>
        /// <param name="password">Hashed password of <see cref="User"/></param>
        /// <param name="level">Level of <see cref="User"/></param>
        /// <param name="experience">Amount of experience the <see cref="User"/> has</param>
        /// <param name="skillPoints">Amount of skill points the <see cref="User"/> has</param>
        /// <param name="alive">Is the <see cref="User"/> alive?</param>
        /// <param name="currentLocation">Location the <see cref="User"/> slept at last</param>
        /// <param name="currentEndurance">Amount of Endurance the <see cref="User"/> currently has</param>
        /// <param name="maximumEndurance">Maximum amount of Endurance the <see cref="User"/> can have at the moment</param>
        /// <param name="hunger">Amount of hunger the <see cref="User"/> currently has</param>
        /// <param name="thirst">Amount of thirst the <see cref="User"/> currently has</param>
        /// <param name="currentWeapon">Type of Weapon the <see cref="User"/> currently has equipped</param>
        /// <param name="lightWeapon">Weapon the <see cref="User"/> has in their Light Weapon slot</param>
        /// <param name="heavyWeapon">Weapon the <see cref="User"/> has in their Heavy Weapon slot</param>
        /// <param name="twoHandedWeapon">Weapon the <see cref="User"/> has in their Two-Handed Weapon slot</param>
        /// <param name="armor">Armor the <see cref="User"/> has equipped</param>
        /// <param name="potion">Potion the <see cref="User"/> is carrying</param>
        /// <param name="lockpicks">Amount of lockpicks the <see cref="User"/> currently has</param>
        /// <param name="goldOnHand">Amount of gold the <see cref="User"/> is currently carrying</param>
        /// <param name="goldInBank">Amount of gold the <see cref="User"/> has stored in the Bank</param>
        /// <param name="goldOnLoan">Amount of unpaid gold the <see cref="User"/> has borrowed from the Bank</param>
        /// <param name="shovel">Does the <see cref="User"/> own a shovel?</param>
        /// <param name="lantern">Does the <see cref="User"/> own a lantern?</param>
        /// <param name="amulet">Does the <see cref="User"/> own an amulet?</param>
        /// <param name="lightWeaponSkill">Amount of skill the <see cref="User"/> has with Light Weapons</param>
        /// <param name="heavyWeaponSkill">Amount of skill the <see cref="User"/> has with Heavy Weapons</param>
        /// <param name="twoHandedWeaponSkill">Amount of skill the <see cref="User"/> has with Two-Handed Weapons</param>
        /// <param name="blocking">Amount of skill the <see cref="User"/> has with blocking incoming attacks</param>
        /// <param name="slipping">Amount of skill the <see cref="User"/> has with dodging attacks and fleeing battles</param>
        /// <param name="stealth">Amount of skill the <see cref="User"/> has with surprising opponents, theft, and hiding</param>
        /// <param name="henchmenLevel1">Amount of Level 1 Henchmen employed by the <see cref="User"/></param>
        /// <param name="henchmenLevel2">Amount of Level 2 Henchmen employed by the <see cref="User"/></param>
        /// <param name="henchmenLevel3">Amount of Level 3 Henchmen employed by the <see cref="User"/></param>
        /// <param name="henchmenLevel4">Amount of Level 4 Henchmen employed by the <see cref="User"/></param>
        /// <param name="henchmenLevel5">Amount of Level 5 Henchmen employed by the <see cref="User"/></param>
        public User(string name, string password, int level, int experience, int skillPoints, bool alive, SleepLocation currentLocation, int currentEndurance, int maximumEndurance, int hunger, int thirst, WeaponType currentWeapon, Weapon lightWeapon, Weapon heavyWeapon, Weapon twoHandedWeapon, Armor armor, Potion potion, int lockpicks, int goldOnHand, int goldInBank, int goldOnLoan, bool shovel, bool lantern, bool amulet, int lightWeaponSkill, int heavyWeaponSkill, int twoHandedWeaponSkill, int blocking, int slipping, int stealth, int henchmenLevel1, int henchmenLevel2, int henchmenLevel3, int henchmenLevel4, int henchmenLevel5)
        {
            Name = name;
            Password = password;
            Level = level;
            Experience = experience;
            SkillPoints = skillPoints;
            Alive = alive;
            CurrentLocation = currentLocation;
            CurrentEndurance = currentEndurance;
            MaximumEndurance = maximumEndurance;
            Hunger = hunger;
            Thirst = thirst;

            CurrentWeaponType = currentWeapon;
            LightWeapon = lightWeapon;
            HeavyWeapon = heavyWeapon;
            TwoHandedWeapon = twoHandedWeapon;
            Armor = armor;
            Potion = potion;
            Lockpicks = lockpicks;
            GoldOnHand = goldOnHand;
            GoldInBank = goldInBank;
            GoldOnLoan = goldOnLoan;
            Shovel = shovel;
            Lantern = lantern;
            Amulet = amulet;

            LightWeaponSkill = lightWeaponSkill;
            HeavyWeaponSkill = heavyWeaponSkill;
            TwoHandedWeaponSkill = twoHandedWeaponSkill;
            Blocking = blocking;
            Slipping = slipping;
            Stealth = stealth;

            HenchmenLevel1 = henchmenLevel1;
            HenchmenLevel2 = henchmenLevel2;
            HenchmenLevel3 = henchmenLevel3;
            HenchmenLevel4 = henchmenLevel4;
            HenchmenLevel5 = henchmenLevel5;
        }

        /// <summary>Replaces this instance of <see cref="User"/> with another instance.</summary>
        /// <param name="other"><see cref="User"/> to replace this instance.</param>
        public User(User other) : this(other.Name, other.Password, other.Level, other.Experience, other.SkillPoints, other.Alive, other.CurrentLocation, other.CurrentEndurance, other.MaximumEndurance, other.Hunger, other.Thirst, other.CurrentWeaponType, other.LightWeapon, other.HeavyWeapon, other.TwoHandedWeapon, other.Armor, other.Potion, other.Lockpicks, other.GoldOnHand, other.GoldInBank, other.GoldOnLoan, other.Shovel, other.Lantern, other.Amulet, other.LightWeaponSkill, other.HeavyWeaponSkill, other.TwoHandedWeaponSkill, other.Blocking, other.Slipping, other.Stealth, other.HenchmenLevel1, other.HenchmenLevel2, other.HenchmenLevel3, other.HenchmenLevel4, other.HenchmenLevel5)
        { }

        #endregion Constructors
    }
}