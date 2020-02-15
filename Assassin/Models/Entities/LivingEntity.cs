using Assassin.Models.Items;

namespace Assassin.Models.Entities
{
    public abstract class LivingEntity : BaseINPC, IEntity
    {
        #region Fields

        private string _name;
        private int _level, _currentEndurance, _maximumEndurance, _goldOnHand, _blocking, _slipping;
        private Armor _armor = new Armor();

        #endregion Fields

        #region Modifying Properties

        /// <summary>Name of this Entity.</summary>
        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(nameof(Name)); }
        }

        /// <summary>Level of this Entity.</summary>
        public int Level
        {
            get => _level;
            set { _level = value; NotifyPropertyChanged(nameof(Level)); }
        }

        /// <summary>Entity's current amount of health.</summary>
        public int CurrentEndurance
        {
            get => _currentEndurance;
            set { _currentEndurance = value; NotifyPropertyChanged(nameof(CurrentEndurance), nameof(EnduranceToString), nameof(EnduranceRatio)); }
        }

        /// <summary>Entity's maxmimum amount of health.</summary>
        public int MaximumEndurance
        {
            get => _maximumEndurance;
            set { _maximumEndurance = value; NotifyPropertyChanged(nameof(MaximumEndurance), nameof(EnduranceRatio), nameof(EnduranceToString)); }
        }

        /// <summary>Amount of gold the Entity has in their possession.</summary>
        public int GoldOnHand
        {
            get => _goldOnHand;
            set
            {
                _goldOnHand = value;
                NotifyPropertyChanged(nameof(GoldOnHand), nameof(GoldOnHandToString), nameof(GoldOnHandToStringWithText));
            }
        }

        /// <summary>Entity's Armor.</summary>
        public Armor Armor
        {
            get => _armor;
            set { _armor = value; NotifyPropertyChanged(nameof(Armor)); }
        }

        /// <summary>Entity's skill at blocking.</summary>
        public int Blocking
        {
            get => _blocking;
            set { _blocking = value; NotifyPropertyChanged(nameof(Blocking), nameof(BlockingToString)); }
        }

        /// <summary>Entity's skill at dodging and running away.</summary>
        public int Slipping
        {
            get => _slipping;
            set { _slipping = value; NotifyPropertyChanged(nameof(Slipping), nameof(SlippingToString)); }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Entity's current amount of health as opposed to their maximum health, formatted.</summary>
        public string EnduranceToString => $"{CurrentEndurance:N0} / {MaximumEndurance:N0}";

        /// <summary>Ratio of current to maximum endurance.</summary>
        public decimal EnduranceRatio => decimal.Divide(CurrentEndurance, MaximumEndurance);

        /// <summary>Amount of gold the Entity has in their possession, formatted.</summary>
        public string GoldOnHandToString => GoldOnHand.ToString("N0");

        /// <summary>Amount of gold the Entity has in their possession, formatted, with preceding text.</summary>
        public string GoldOnHandToStringWithText => $"Gold on Hand: {GoldOnHandToString}";

        /// <summary>Entity's skill at blocking, formatted.</summary>
        public string BlockingToString => $"{Blocking:N0}%";

        /// <summary>Entity's skill at dodging and running away.</summary>
        public string SlippingToString => $"{Slipping:N0}%";

        #endregion Helper Properties

        #region Health Manipulation

        /// <summary>The Entity takes damage.</summary>
        /// <param name="damage">Amount of damage taken.</param>
        /// <returns>Message regarding damage taken</returns>
        public abstract string TakeDamage(int damage);

        #endregion Health Manipulation

        public override string ToString() => Name;
    }
}