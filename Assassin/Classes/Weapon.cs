using System;
using System.ComponentModel;

namespace Assassin
{
    public enum WeaponType { Light, Heavy, TwoHanded }

    public class Weapon : IEquatable<Weapon>, INotifyPropertyChanged
    {
        private string _name;
        private int _damage, _value;
        private bool _hidden;
        private WeaponType _type;

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public WeaponType Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; OnPropertyChanged("Damage"); }
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

        public static bool Equals(Weapon left, Weapon right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return (left.Name == right.Name) && (left.Type == right.Type) && (left.Damage == right.Damage) && (left.Value == right.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as Weapon);
        }

        public bool Equals(Weapon otherWeapon)
        {
            return Equals(this, otherWeapon);
        }

        public static bool operator ==(Weapon left, Weapon right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Weapon left, Weapon right)
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
        /// Initializes a new instance of the Weapon class.
        /// </summary>
        public Weapon()
        {
            Name = "Hands";
            Type = WeaponType.Light;
            Damage = 6;
            Value = 0;
        }

        /// <summary>
        /// Initializes a new instance of the Weapon class using Property values.
        /// </summary>
        /// <param name="name">Name of weapon</param>
        /// <param name="type">Type of weapon</param>
        /// <param name="damage">Damage value of weapon</param>
        /// <param name="value">Gold value of weapon</param>
        /// <param name="hidden">Hidden from sale</param>
        public Weapon(string name, WeaponType type, int damage, int value, bool hidden)
        {
            Name = name;
            Type = type;
            Damage = damage;
            Value = value;
            Hidden = hidden;
        }

        /// <summary>
        /// Initializes a new instance of the Weapon class using another Weapon.
        /// </summary>
        /// <param name="otherWeapon">Weapon to replace this instance</param>
        public Weapon(Weapon otherWeapon)
        {
            Name = otherWeapon.Name;
            Type = otherWeapon.Type;
            Damage = otherWeapon.Damage;
            Value = otherWeapon.Value;
            Hidden = otherWeapon.Hidden;
        }

        #endregion Constructors
    }
}