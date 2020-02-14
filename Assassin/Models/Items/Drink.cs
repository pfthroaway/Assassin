using System;

namespace Assassin.Models.Items
{
    /// <summary>Represents a <see cref="Drink"/> to relieve thirst.</summary>
    public class Drink : Item, IEquatable<Drink>
    {
        private int _restoreThirst;

        #region Properties

        /// <summary>Amount of thirst restored by this <see cref="Drink"/>.</summary>
        public int RestoreThirst
        {
            get => _restoreThirst;
            set { _restoreThirst = value; NotifyPropertyChanged(nameof(RestoreThirst)); }
        }

        #endregion Properties

        #region Override Operators

        public static bool Equals(Drink left, Drink right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && left.RestoreThirst == right.RestoreThirst && left.Value == right.Value && left.Hidden == right.Hidden;
        }

        public override bool Equals(object obj) => Equals(this, obj as Drink);

        public bool Equals(Drink other) => Equals(this, other);

        public static bool operator ==(Drink left, Drink right) => Equals(left, right);

        public static bool operator !=(Drink left, Drink right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary> Initializes a new instance of the <see cref="Drink"/> class.</summary>
        internal Drink()
        {
            Name = "";
            RestoreThirst = 0;
            Value = 0;
            Hidden = false;
        }

        /// <summary>Initializes a new instance of the <see cref="Drink"/> class using Property values.</summary>
        /// <param name="name">Name of <see cref="Drink"/></param>
        /// <param name="restoreThirst">Amount of Thirst restored by <see cref="Drink"/></param>
        /// <param name="value">Gold value of <see cref="Drink"/></param>
        internal Drink(string name, int restoreThirst, int value)
        {
            Name = name;
            RestoreThirst = restoreThirst;
            Value = value;
        }

        /// <summary>Replaces this instance of <see cref="Drink"/> with another instance.</summary>
        /// <param name="other"><see cref="Drink"/> to replace this instance</param>
        internal Drink(Drink other) : this(other.Name, other.RestoreThirst, other.Value)
        {
        }

        #endregion Constructors
    }
}