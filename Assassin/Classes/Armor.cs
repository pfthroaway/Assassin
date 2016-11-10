using System;
using System.ComponentModel;

namespace Assassin
{
    public class Armor : IEquatable<Armor>, INotifyPropertyChanged
    {
        private string _name;
        private int _defense, _value;
        private bool _hidden;

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public int Defense
        {
            get { return _defense; }
            set { _defense = value; OnPropertyChanged("Defense"); }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; OnPropertyChanged("Hidden"); }
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

        public static bool Equals(Armor left, Armor right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return (left.Name == right.Name) && (left.Defense == right.Defense) && (left.Value == right.Value) && (left.Hidden == right.Hidden);
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as Armor);
        }

        public bool Equals(Armor otherArmor)
        {
            return Equals(this, otherArmor);
        }

        public static bool operator ==(Armor left, Armor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Armor left, Armor right)
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
        /// Initializes a new instance of the Armor class.
        /// </summary>
        internal Armor()
        {
            Name = "Clothes";
            Defense = 4;
            Value = 0;
            Hidden = false;
        }

        /// <summary>
        /// Initializes a new instance of the Armor class using Property values.
        /// </summary>
        /// <param name="name">Name of Armor</param>
        /// <param name="defense">Defense value of Armor</param>
        /// <param name="value">Gold value of Armor</param>
        /// <param name="hidden">Hidden from sale</param>
        internal Armor(string name, int defense, int value, bool hidden)
        {
            Name = name;
            Defense = defense;
            Value = value;
            Hidden = hidden;
        }

        /// <summary>
        /// Initializes a new instance of the Armor class using another Armor.
        /// </summary>
        /// <param name="otherArmor">Armor to replace this instance</param>
        internal Armor(Armor otherArmor)
        {
            Name = otherArmor.Name;
            Defense = otherArmor.Defense;
            Value = otherArmor.Value;
            Hidden = otherArmor.Hidden;
        }

        #endregion Constructors
    }
}