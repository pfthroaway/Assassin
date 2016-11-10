using System.ComponentModel;

namespace Assassin
{
    public class Potion
    {
        private string _name;
        private int _healAmount, _value;

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public int HealAmount
        {
            get { return _healAmount; }
            set { _healAmount = value; OnPropertyChanged("HealAmount"); }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Override Operators

        public static bool Equals(Potion left, Potion right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return (left.Name == right.Name) && (left.HealAmount == right.HealAmount) && (left.Value == right.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as Potion);
        }

        public bool Equals(Potion otherPotion)
        {
            return Equals(this, otherPotion);
        }

        public static bool operator ==(Potion left, Potion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Potion left, Potion right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ 17;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Override Operators

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Potion class.
        /// </summary>
        internal Potion()
        {
            Name = "None";
            HealAmount = 0;
            Value = 0;
        }

        /// <summary>
        /// Initializes a new instance of the Potion class using Property values.
        /// </summary>
        /// <param name="name">Name of Potion</param>
        /// <param name="healAmount">Amount the Potion heals the User</param>
        /// <param name="value">Gold value of Potion</param>
        internal Potion(string name, int healAmount, int value)
        {
            Name = name;
            HealAmount = healAmount;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the Potion using another Potion.
        /// </summary>
        /// <param name="otherPotion">Potion to replace this instance</param>
        internal Potion(Potion otherPotion)
        {
            Name = otherPotion.Name;
            HealAmount = otherPotion.HealAmount;
            Value = otherPotion.Value;
        }

        #endregion Constructors
    }
}