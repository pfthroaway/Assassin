using System.ComponentModel;

namespace Assassin
{
    internal class Enemy : INotifyPropertyChanged
    {
        private string _name;
        private int _level, _currentEndurance, _maximumEndurance, _goldOnHand, _weaponSkill, _blocking, _slipping;
        private Weapon _weapon = new Weapon();
        private Armor _armor = new Armor();

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged("Level"); }
        }

        public int CurrentEndurance
        {
            get { return _currentEndurance; }
            set { _currentEndurance = value; OnPropertyChanged("EnduranceToString"); }
        }

        public int MaximumEndurance
        {
            get { return _maximumEndurance; }
            set { _maximumEndurance = value; OnPropertyChanged("EnduranceToString"); }
        }

        public string EnduranceToString
        {
            get { return CurrentEndurance.ToString("N0") + " / " + MaximumEndurance.ToString("N0"); }
        }

        public Weapon Weapon
        {
            get { return _weapon; }
            set { _weapon = value; OnPropertyChanged("Weapon"); }
        }

        public Armor Armor
        {
            get { return _armor; }
            set { _armor = value; OnPropertyChanged("Armor"); }
        }

        public int GoldOnHand
        {
            get { return _goldOnHand; }
            set { _goldOnHand = value; OnPropertyChanged("GoldOnHand"); }
        }

        public int WeaponSkill
        {
            get { return _weaponSkill; }
            set { _weaponSkill = value; OnPropertyChanged("WeaponSkill"); }
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

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Health Manipulation

        /// <summary>
        /// The Enemy takes damage.
        /// </summary>
        /// <param name="damage">Amount of damage taken.</param>
        /// <returns>String saying the Enemy took damage</returns>
        internal string TakeDamage(int damage)
        {
            CurrentEndurance -= damage;
            return "The " + Name + " takes " + damage + " damage.";
        }

        #endregion Health Manipulation

        public override string ToString()
        {
            return Name;
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Enemy class.
        /// </summary>
        internal Enemy()
        {
            Name = "";
            Level = 1;
            CurrentEndurance = 100;
            MaximumEndurance = 100;
            Weapon = new Weapon();
            Armor = new Armor();
            GoldOnHand = 100;
            WeaponSkill = 10;
            Blocking = 10;
            Slipping = 10;
        }

        /// <summary>
        /// Initializes a new instance of the Enemy class by assigning Property values.
        /// </summary>
        /// <param name="name">Name of Enemy</param>
        /// <param name="currentEndurance">Amount of Endurance the Enemy currently has</param>
        /// <param name="maximumEndurance">Maximum amount of Endurance the Enemy can have</param>
        /// <param name="weapon">Weapon equipped by the Enemy</param>
        /// <param name="armor">Armor equipped by the Enemy</param>
        /// <param name="goldOnHand">Amount of Gold the Enemy is currently carrying</param>
        /// <param name="weaponSkill">Amount of skill the Enemy has with their Weapon</param>
        /// <param name="blocking">Amount of skill the Enemy has with blocking incoming attacks</param>
        /// <param name="slipping">Amount of skill the Enemy has with dodging attacks and fleeing battles</param>
        internal Enemy(string name, int level, int currentEndurance, int maximumEndurance, Weapon weapon, Armor armor, int goldOnHand, int weaponSkill, int blocking, int slipping)
        {
            Name = name;
            Level = level;
            CurrentEndurance = currentEndurance;
            MaximumEndurance = maximumEndurance;
            Weapon = weapon;
            Armor = armor;
            GoldOnHand = goldOnHand;
            WeaponSkill = weaponSkill;
            Blocking = blocking;
            Slipping = slipping;
        }

        /// <summary>
        /// Replaces this instance of Enemy with another instance.
        /// </summary>
        /// <param name="otherEnemy">Enemy to replace this instance</param>
        internal Enemy(Enemy otherEnemy)
        {
            Name = otherEnemy.Name;
            Level = otherEnemy.Level;
            CurrentEndurance = otherEnemy.CurrentEndurance;
            MaximumEndurance = otherEnemy.MaximumEndurance;
            Weapon = otherEnemy.Weapon;
            Armor = otherEnemy.Armor;
            GoldOnHand = otherEnemy.GoldOnHand;
            WeaponSkill = otherEnemy.WeaponSkill;
            Blocking = otherEnemy.Blocking;
            Slipping = otherEnemy.Slipping;
        }

        #endregion Constructors
    }
}