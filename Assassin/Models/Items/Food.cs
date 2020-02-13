using System;

namespace Assassin.Models.Items
{
    /// <summary>Represents <see cref="Food"/> used to ease hunger.</summary>
    public class Food : Item, IEquatable<Food>
    {
        private int _restoreHunger;

        #region Properties

        /// <summary>Amount of hunger restored by this Food.</summary>
        public int RestoreHunger
        {
            get => _restoreHunger;
            set { _restoreHunger = value; OnPropertyChanged("RestoreHunger"); }
        }

        #endregion Properties

        #region Override Operators

        public static bool Equals(Food left, Food right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && left.RestoreHunger == right.RestoreHunger && left.Value == right.Value && left.Hidden == right.Hidden;
        }

        public override bool Equals(object obj) => Equals(this, obj as Food);

        public bool Equals(Food other) => Equals(this, other);

        public static bool operator ==(Food left, Food right) => Equals(left, right);

        public static bool operator !=(Food left, Food right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Food"/> class.</summary>
        internal Food()
        {
            Name = "";
            RestoreHunger = 0;
            Value = 0;
            Hidden = false;
        }

        /// <summary>Initializes a new instance of the <see cref="Food"/> class using Property values.</summary>
        /// <param name="name">Name of <see cref="Food"/></param>
        /// <param name="restoreHunger">Amount of Hunger restored by <see cref="Food"/></param>
        /// <param name="value">Gold value of <see cref="Food"/></param>
        internal Food(string name, int restoreHunger, int value, bool hidden = false)
        {
            Name = name;
            RestoreHunger = restoreHunger;
            Value = value;
            Hidden = hidden;
        }

        /// <summary>Replaces this instance of <see cref="Food"/> with another instance.</summary>
        /// <param name="other"><see cref="Food"/> to replace this instance</param>
        internal Food(Food other) : this(other.Name, other.RestoreHunger, other.Value, other.Hidden)
        {
        }

        #endregion Constructors
    }
}