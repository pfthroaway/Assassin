using System.ComponentModel;

namespace Assassin
{
    public class User : INotifyPropertyChanged
    {
        private string _name, _password, _currentLocation;
        private int _level, _experience, _skillPoints, _currentEndurance, _maximumEndurance, _hunger, _thirst, _lockpicks, _goldOnHand, _goldInBank, _goldOnLoan, _lightWeaponSkill, _heavyWeaponSkill, _twoHandedWeaponSkill, _blocking, _slipping, _stealth, _henchmenLevel1, _henchmenLevel2, _henchmenLevel3, _henchmenLevel4, _henchmenLevel5;
        private bool _alive, _shovel, _lantern, _amulet;
        private WeaponType _currentWeapon;
        private Weapon _lightWeapon, _heavyWeapon, _twoHandedWeapon;
        private Armor _armor = new Armor();
        private Potion _potion;

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged("Level"); }
        }

        public string Rank
        {
            get { return GameState.AllRanks[Level]; }
        }

        public int Experience
        {
            get { return _experience; }
            set { _experience = value; OnPropertyChanged("Experience"); }
        }

        public int SkillPoints
        {
            get { return _skillPoints; }
            set { _skillPoints = value; OnPropertyChanged("SkillPoints"); OnPropertyChanged("SkillPointsToString"); }
        }

        public string SkillPointsToString
        {
            get { return SkillPoints.ToString("N0") + " Skill Points Available"; }
        }

        public bool Alive
        {
            get { return _alive; }
            set { _alive = value; OnPropertyChanged("Alive"); }
        }

        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; OnPropertyChanged("CurrentLocation"); }
        }

        public int CurrentEndurance
        {
            get { return _currentEndurance; }
            set { _currentEndurance = value; OnPropertyChanged("EnduranceToString"); }
        }

        public int MaximumEndurance
        {
            get { return _maximumEndurance; }
            set { _maximumEndurance = value; OnPropertyChanged("MaximumEndurance"); OnPropertyChanged("EnduranceToString"); }
        }

        public string EnduranceToString
        {
            get { return CurrentEndurance.ToString("N0") + " / " + MaximumEndurance.ToString("N0"); }
        }

        public int Hunger
        {
            get { return _hunger; }
            set { _hunger = value; OnPropertyChanged("Hunger"); }
        }

        public int Thirst
        {
            get { return _thirst; }
            set { _thirst = value; OnPropertyChanged("Thirst"); }
        }

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

        public WeaponType CurrentWeapon
        {
            get { return _currentWeapon; }
            set { _currentWeapon = value; OnPropertyChanged("SelectedWeapon"); }
        }

        public Weapon SelectedWeapon
        {
            get
            {
                Weapon newWeapon = new Weapon();
                switch (CurrentWeapon)
                {
                    case WeaponType.Light:
                        newWeapon = LightWeapon;
                        break;

                    case WeaponType.Heavy:
                        newWeapon = HeavyWeapon;
                        break;

                    case WeaponType.TwoHanded:
                        newWeapon = TwoHandedWeapon;
                        break;
                }
                return newWeapon;
            }
        }

        public int SelectedWeaponSkill
        {
            get
            {
                int skill = 0;
                switch (CurrentWeapon)
                {
                    case WeaponType.Light:
                        skill = LightWeaponSkill;
                        break;

                    case WeaponType.Heavy:
                        skill = HeavyWeaponSkill;
                        break;

                    case WeaponType.TwoHanded:
                        skill = TwoHandedWeaponSkill;
                        break;
                }
                return skill;
            }
        }

        public Weapon LightWeapon
        {
            get { return _lightWeapon; }
            set { _lightWeapon = value; OnPropertyChanged("LightWeapon"); }
        }

        public Weapon HeavyWeapon
        {
            get { return _heavyWeapon; }
            set { _heavyWeapon = value; OnPropertyChanged("HeavyWeapon"); }
        }

        public Weapon TwoHandedWeapon
        {
            get { return _twoHandedWeapon; }
            set { _twoHandedWeapon = value; OnPropertyChanged("TwoHandedWeapon"); }
        }

        public Armor Armor
        {
            get { return _armor; }
            set { _armor = value; OnPropertyChanged("Armor"); }
        }

        public Potion Potion
        {
            get { return _potion; }
            set { _potion = value; OnPropertyChanged("Potion"); }
        }

        public int Lockpicks
        {
            get { return _lockpicks; }
            set { _lockpicks = value; OnPropertyChanged("Lockpicks"); }
        }

        public bool Shovel
        {
            get { return _shovel; }
            set { _shovel = value; OnPropertyChanged("Shovel"); }
        }

        public bool Lantern
        {
            get { return _lantern; }
            set { _lantern = value; OnPropertyChanged("Lantern"); }
        }

        public bool Amulet
        {
            get { return _amulet; }
            set { _amulet = value; OnPropertyChanged("Amulet"); }
        }

        public int GoldOnHand
        {
            get { return _goldOnHand; }
            set { _goldOnHand = value; OnPropertyChanged("GoldOnHand"); }
        }

        public int GoldInBank
        {
            get { return _goldInBank; }
            set { _goldInBank = value; OnPropertyChanged("GoldInBank"); }
        }

        public int GoldOnLoan
        {
            get { return _goldOnLoan; }
            set { _goldOnLoan = value; OnPropertyChanged("GoldOnLoan"); OnPropertyChanged("LoanAvailable"); OnPropertyChanged("LoanAvailableToString"); }
        }

        public string GoldInBankToString
        {
            get { return GoldInBank.ToString("N0"); }
        }

        public string GoldOnLoanToString
        {
            get { return GoldOnLoan.ToString("N0"); }
        }

        public string GoldOnHandToString
        {
            get { return GoldOnHand.ToString("N0"); }
        }

        public int LoanAvailable
        {
            get { return (Level * 250) - GoldOnLoan; }
        }

        public string LoanAvailableToString
        {
            get { return ((Level * 250) - GoldOnLoan).ToString("N0"); }
        }

        public int LightWeaponSkill
        {
            get { return _lightWeaponSkill; }
            set { _lightWeaponSkill = value; OnPropertyChanged("LightWeaponSkill"); }
        }

        public int HeavyWeaponSkill
        {
            get { return _heavyWeaponSkill; }
            set { _heavyWeaponSkill = value; OnPropertyChanged("HeavyWeaponSkill"); }
        }

        public int TwoHandedWeaponSkill
        {
            get { return _twoHandedWeaponSkill; }
            set { _twoHandedWeaponSkill = value; OnPropertyChanged("TwoHandedWeaponSkill"); }
        }

        public int Blocking
        {
            get { return _blocking; }
            set { _blocking = value; OnPropertyChanged("Blocking"); }
        }

        public int Slipping
        {
            get { return _slipping; }
            set { _slipping = value; OnPropertyChanged("Slipping"); }
        }

        public int Stealth
        {
            get { return _stealth; }
            set { _stealth = value; OnPropertyChanged("Stealth"); }
        }

        public int HenchmenLevel1
        {
            get { return _henchmenLevel1; }
            set { _henchmenLevel1 = value; OnPropertyChanged("HenchmenLevel1"); }
        }

        public int HenchmenLevel2
        {
            get { return _henchmenLevel2; }
            set { _henchmenLevel2 = value; OnPropertyChanged("HenchmenLevel2"); }
        }

        public int HenchmenLevel3
        {
            get { return _henchmenLevel3; }
            set { _henchmenLevel3 = value; OnPropertyChanged("HenchmenLevel3"); }
        }

        public int HenchmenLevel4
        {
            get { return _henchmenLevel4; }
            set { _henchmenLevel4 = value; OnPropertyChanged("HenchmenLevel4"); }
        }

        public int HenchmenLevel5
        {
            get { return _henchmenLevel5; }
            set { _henchmenLevel5 = value; OnPropertyChanged("HenchmenLevel5"); }
        }

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        /// <summary>
        /// Gains experience for the User.
        /// </summary>
        /// <param name="experience">Experience gained</param>
        /// <returns>String based on experience gain</returns>
        internal string GainExperience(int experience)
        {
            int oldExperience = Experience;
            string ExperienceText = "You have earned " + experience + " experience from the battle."; ;
            string LevelText = "";

            Experience += experience;
            if (oldExperience / 10 < Experience / 10)
            {
                Level += 1;
                LevelText = " You have gained a level! You are now a " + GameState.AllRanks[Level] + "!";
            }

            //if past maximum exp
            if (Experience > 100)
            {
                Experience = 100;
                //set to maximum
            }

            return ExperienceText + LevelText;
        }

        #region Health Manipulation

        /// <summary>
        /// The User heals damage.
        /// </summary>
        /// <param name="healAmount">Amount of health restored.</param>
        /// <returns>String saying you took damage</returns>
        internal string Heal(int healAmount)
        {
            CurrentEndurance += healAmount;
            if (CurrentEndurance > MaximumEndurance)
                CurrentEndurance = MaximumEndurance;
            return "You heal for " + healAmount + " damage.";
        }

        /// <summary>
        /// The User takes damage.
        /// </summary>
        /// <param name="healAmount">Amount of damage taken.</param>
        /// <returns>String saying you took damage</returns>
        internal string TakeDamage(int damage)
        {
            CurrentEndurance -= damage;
            return "You take " + damage + " damage.";
        }

        #endregion Health Manipulation

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the User class.
        /// </summary>
        internal User()
        {
            Name = "";
            Password = "";
            Level = 1;
            Experience = 0;
            SkillPoints = 5;
            Alive = true;
            CurrentLocation = "Streets";
            CurrentEndurance = 100;
            MaximumEndurance = 100;
            Hunger = 0;
            Thirst = 0;

            CurrentWeapon = WeaponType.Light;
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

        /// <summary>
        /// Initializes a new instance of the User class using Property values.
        /// </summary>
        /// <param name="name">Name of User</param>
        /// <param name="password">Hashed password of User</param>
        /// <param name="level">Level of User</param>
        /// <param name="experience">Amount of experience the User has</param>
        /// <param name="skillPoints">Amount of skill points the User has</param>
        /// <param name="alive">Is the User alive?</param>
        /// <param name="currentLocation">Location the User slept at last</param>
        /// <param name="currentEndurance">Amount of Endurance the User currently has</param>
        /// <param name="maximumEndurance">Maximum amount of Endurance the User can have at the moment</param>
        /// <param name="hunger">Amount of hunger the User currently has</param>
        /// <param name="thirst">Amount of thirst the User currently has</param>
        /// <param name="currentWeapon">Type of Weapon the User currently has equipped</param>
        /// <param name="lightWeapon">Weapon the User has in their Light Weapon slot</param>
        /// <param name="heavyWeapon">Weapon the User has in their Heavy Weapon slot</param>
        /// <param name="twoHandedWeapon">Weapon the User has in their Two-Handed Weapon slot</param>
        /// <param name="armor">Armor the User has equipped</param>
        /// <param name="potion">Potion the User is carrying</param>
        /// <param name="lockpicks">Amount of lockpicks the User currently has</param>
        /// <param name="goldOnHand">Amount of gold the User is currently carrying</param>
        /// <param name="goldInBank">Amount of gold the User has stored in the Bank</param>
        /// <param name="goldOnLoan">Amount of unpaid gold the User has borrowed from the Bank</param>
        /// <param name="shovel">Does the User own a shovel?</param>
        /// <param name="lantern">Does the User own a lantern?</param>
        /// <param name="amulet">Does the User own an amulet?</param>
        /// <param name="lightWeaponSkill">Amount of skill the User has with Light Weapons</param>
        /// <param name="heavyWeaponSkill">Amount of skill the User has with Heavy Weapons</param>
        /// <param name="twoHandedWeaponSkill">Amount of skill the User has with Two-Handed Weapons</param>
        /// <param name="blocking">Amount of skill the User has with blocking incoming attacks</param>
        /// <param name="slipping">Amount of skill the User has with dodging attacks and fleeing battles</param>
        /// <param name="stealth">Amount of skill the User has with surprising opponents, theft, and hiding</param>
        /// <param name="henchmenLevel1">Amount of Level 1 Henchmen employed by the User</param>
        /// <param name="henchmenLevel2">Amount of Level 2 Henchmen employed by the User</param>
        /// <param name="henchmenLevel3">Amount of Level 3 Henchmen employed by the User</param>
        /// <param name="henchmenLevel4">Amount of Level 4 Henchmen employed by the User</param>
        /// <param name="henchmenLevel5">Amount of Level 5 Henchmen employed by the User</param>
        internal User(string name, string password, int level, int experience, int skillPoints, bool alive, string currentLocation, int currentEndurance, int maximumEndurance, int hunger, int thirst, WeaponType currentWeapon, Weapon lightWeapon, Weapon heavyWeapon, Weapon twoHandedWeapon, Armor armor, Potion potion, int lockpicks, int goldOnHand, int goldInBank, int goldOnLoan, bool shovel, bool lantern, bool amulet, int lightWeaponSkill, int heavyWeaponSkill, int twoHandedWeaponSkill, int blocking, int slipping, int stealth, int henchmenLevel1, int henchmenLevel2, int henchmenLevel3, int henchmenLevel4, int henchmenLevel5)
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

            CurrentWeapon = currentWeapon;
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

        /// <summary>
        /// Initializes a new instance of the User class using another User.
        /// </summary>
        /// <param name="otherUser">User to replace this instance.</param>
        internal User(User otherUser)
        {
            Name = otherUser.Name;
            Password = otherUser.Password;
            Level = otherUser.Level;
            Experience = otherUser.Experience;
            SkillPoints = otherUser.SkillPoints;
            Alive = otherUser.Alive;
            CurrentLocation = otherUser.CurrentLocation;
            CurrentEndurance = otherUser.CurrentEndurance;
            MaximumEndurance = otherUser.MaximumEndurance;
            Hunger = otherUser.Hunger;
            Thirst = otherUser.Thirst;

            CurrentWeapon = otherUser.CurrentWeapon;
            LightWeapon = otherUser.LightWeapon;
            HeavyWeapon = otherUser.HeavyWeapon;
            TwoHandedWeapon = otherUser.TwoHandedWeapon;
            Armor = otherUser.Armor;
            Potion = otherUser.Potion;
            Lockpicks = otherUser.Lockpicks;
            GoldOnHand = otherUser.GoldOnHand;
            GoldInBank = otherUser.GoldInBank;
            GoldOnLoan = otherUser.GoldOnLoan;
            Shovel = otherUser.Shovel;
            Lantern = otherUser.Lantern;
            Amulet = otherUser.Amulet;

            LightWeaponSkill = otherUser.LightWeaponSkill;
            HeavyWeaponSkill = otherUser.HeavyWeaponSkill;
            TwoHandedWeaponSkill = otherUser.TwoHandedWeaponSkill;
            Blocking = otherUser.Blocking;
            Slipping = otherUser.Slipping;
            Stealth = otherUser.Stealth;

            HenchmenLevel1 = otherUser.HenchmenLevel1;
            HenchmenLevel2 = otherUser.HenchmenLevel2;
            HenchmenLevel3 = otherUser.HenchmenLevel3;
            HenchmenLevel4 = otherUser.HenchmenLevel4;
            HenchmenLevel5 = otherUser.HenchmenLevel5;
        }

        #endregion Constructors
    }
}