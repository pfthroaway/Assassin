using System;

namespace Assassin.Classes.Items
{
    /// <summary>Represents a set of <see cref="Armor"/> worn by an Entity.</summary>
    public class Armor : Item, IEquatable<Armor>
    {
        private int _defense;

        #region Modifying Properties

        /// <summary>Defense of the <see cref="Armor"/>.</summary>
        public int Defense
        {
            get => _defense;
            set
            {
                _defense = value;
                OnPropertyChanged("Defense");
                OnPropertyChanged("DefenseToString");
                OnPropertyChanged("DefenseToStringWithText");
            }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Defense of the <see cref="Armor"/>, formatted.</summary>
        public string DefenseToString => Defense.ToString("N0");

        /// <summary>Defense of the <see cref="Armor"/>, formatted with text.</summary>
        public string DefenseToStringWithText => $"Defense: {DefenseToString}";

        #endregion Helper Properties

        #region Override Operators

        public static bool Equals(Armor left, Armor right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && left.Defense == right.Defense && left.Value == right.Value && left.Hidden == right.Hidden;
        }

        public override bool Equals(object obj) => Equals(this, obj as Armor);

        public bool Equals(Armor other) => Equals(this, other);

        public static bool operator ==(Armor left, Armor right) => Equals(left, right);

        public static bool operator !=(Armor left, Armor right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Armor"/> class.</summary>
        internal Armor()
        {
            Name = "Clothes";
            Defense = 4;
            Value = 0;
            Hidden = false;
        }

        /// <summary>Initializes a new instance of the <see cref="Armor"/> class using Property values.</summary>
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

        /// <summary>Replaces this instance of <see cref="Armor"/> with another instance.</summary>
        /// <param name="otherArmor"><see cref="Armor"/> to replace this instance</param>
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