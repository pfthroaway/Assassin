using Assassin.Models.Enums;
using System;

namespace Assassin.Models.Items
{
    /// <summary>Represents a <see cref="Weapon"/> used to attack.</summary>
    public class Weapon : Item, IEquatable<Weapon>
    {
        private int _damage;
        private WeaponType _type;

        #region Modifying Properties

        /// <summary>Type of <see cref="Weapon"/></summary>
        public WeaponType Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged("Type"); }
        }

        /// <summary>Damage of <see cref="Weapon"/>.</summary>
        public int Damage
        {
            get => _damage;
            set
            {
                _damage = value;
                OnPropertyChanged("Damage");
                OnPropertyChanged("DamageToString");
                OnPropertyChanged("DamageToStringWithText");
            }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Damage of <see cref="Weapon"/>, formatted.</summary>
        public string DamageToString => Damage.ToString("N0");

        /// <summary>Damage of <see cref="Weapon"/>, formatted with text.</summary>
        public string DamageToStringWithText => $"Damage: {DamageToString}";

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Weapon left, Weapon right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && left.Type == right.Type && left.Damage == right.Damage && left.Value == right.Value && left.Hidden == right.Hidden;
        }

        public override bool Equals(object obj) => Equals(this, obj as Weapon);

        public bool Equals(Weapon other) => Equals(this, other);

        public static bool operator ==(Weapon left, Weapon right) => Equals(left, right);

        public static bool operator !=(Weapon left, Weapon right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Weapon"/> class.</summary>
        public Weapon()
        {
            Name = "Hands";
            Type = WeaponType.Light;
            Damage = 6;
            Value = 0;
            Hidden = true;
        }

        /// <summary>Initializes a new instance of the <see cref="Weapon"/> class using Property values.</summary>
        /// <param name="name">Name of <see cref="Weapon"/></param>
        /// <param name="type">Type of <see cref="Weapon"/></param>
        /// <param name="damage">Damage value of <see cref="Weapon"/></param>
        /// <param name="value">Gold value of <see cref="Weapon"/></param>
        /// <param name="hidden">Hidden from sale</param>
        public Weapon(string name, WeaponType type, int damage, int value, bool hidden)
        {
            Name = name;
            Type = type;
            Damage = damage;
            Value = value;
            Hidden = hidden;
        }

        /// <summary>Replaces this instance of <see cref="Weapon"/> with another instance.</summary>
        /// <param name="other"><see cref="Weapon"/> to replace this instance</param>
        public Weapon(Weapon other) : this(other.Name, other.Type, other.Damage, other.Value, other.Hidden)
        {
        }

        #endregion Constructors
    }
}