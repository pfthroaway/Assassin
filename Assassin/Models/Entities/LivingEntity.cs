using Assassin.Models.Items;
using System.ComponentModel;

namespace Assassin.Models.Entities
{
    public abstract class LivingEntity : IEntity, INotifyPropertyChanged
    {
        #region Fields

        private string _name;
        private int _level, _currentEndurance, _maximumEndurance, _goldOnHand, _blocking, _slipping;
        private Armor _armor = new Armor();

        #endregion Fields

        #region Modifying Properties Properties

        /// <summary>Name of this Entity.</summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>Level of this Entity.</summary>
        public int Level
        {
            get => _level;
            set { _level = value; OnPropertyChanged("Level"); }
        }

        /// <summary>Entity's current amount of health.</summary>
        public int CurrentEndurance
        {
            get => _currentEndurance;
            set { _currentEndurance = value; OnPropertyChanged("EnduranceToString"); }
        }

        /// <summary>Entity's maxmimum amount of health.</summary>
        public int MaximumEndurance
        {
            get => _maximumEndurance;
            set { _maximumEndurance = value; OnPropertyChanged("MaximumEndurance"); OnPropertyChanged("EnduranceToString"); }
        }

        /// <summary>Amount of gold the Entity has in their possession.</summary>
        public int GoldOnHand
        {
            get => _goldOnHand;
            set
            {
                _goldOnHand = value;
                OnPropertyChanged("GoldOnHand");
                OnPropertyChanged("GoldOnHandToString");
                OnPropertyChanged("GoldOnHandToStringWithText");
            }
        }

        /// <summary>Entity's Armor.</summary>
        public Armor Armor
        {
            get => _armor;
            set { _armor = value; OnPropertyChanged("Armor"); }
        }

        #endregion Modifying Properties Properties

        #region Helper Properties

        /// <summary>Entity's current amount of health as opposed to their maximum health, formatted.</summary>
        public string EnduranceToString => $"{CurrentEndurance:N0} / {MaximumEndurance:N0}";

        /// <summary>Amount of gold the Entity has in their possession, formatted.</summary>
        public string GoldOnHandToString => GoldOnHand.ToString("N0");

        /// <summary>Amount of gold the Entity has in their possession, formatted, with preceding text.</summary>
        public string GoldOnHandToStringWithText => $"Gold on Hand: {GoldOnHandToString}";

        /// <summary>Entity's skill at blocking.</summary>
        public int Blocking
        {
            get => _blocking;
            set { _blocking = value; OnPropertyChanged("Blocking"); }
        }

        /// <summary>Entity's skill at dodging and running away.</summary>
        public int Slipping
        {
            get => _slipping;
            set { _slipping = value; OnPropertyChanged("Slipping"); }
        }

        #endregion Helper Properties

        #region Health Manipulation

        /// <summary>The Entity takes damage.</summary>
        /// <param name="damage">Amount of damage taken.</param>
        /// <returns>Message regarding damage taken</returns>
        public abstract string TakeDamage(int damage);

        #endregion Health Manipulation

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        public override string ToString() => Name;
    }
}