using System;

namespace Assassin.Models.Items
{
    /// <summary>Represents a Potion that is drinkable by a User.</summary>
    public class Potion : Item, IEquatable<Potion>
    {
        private int _healAmount;

        #region Properties

        /// <summary>Amount of health this <see cref="Potion"/> heals.</summary>
        public int HealAmount
        {
            get => _healAmount;
            set { _healAmount = value; NotifyPropertyChanged(nameof(HealAmount)); }
        }

        #endregion Properties

        #region Override Operators

        public static bool Equals(Potion left, Potion right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && left.HealAmount == right.HealAmount && left.Value == right.Value && left.Hidden == right.Hidden;
        }

        public override bool Equals(object obj) => Equals(this, obj as Potion);

        public bool Equals(Potion other) => Equals(this, other);

        public static bool operator ==(Potion left, Potion right) => Equals(left, right);

        public static bool operator !=(Potion left, Potion right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => Name;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Potion"/> class.</summary>
        internal Potion()
        {
            Name = "None";
            HealAmount = 0;
            Value = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="Potion"/> class using Property values.</summary>
        /// <param name="name">Name of <see cref="Potion"/></param>
        /// <param name="healAmount">Amount the <see cref="Potion"/> heals the User</param>
        /// <param name="value">Gold value of <see cref="Potion"/></param>
        /// <param name="hidden">Is this <see cref="Potion"/> hidden?</param>
        internal Potion(string name, int healAmount, int value, bool hidden = false)
        {
            Name = name;
            HealAmount = healAmount;
            Value = value;
            Hidden = hidden;
        }

        /// <summary>Replaces this instance of <see cref="Potion"/> with another instance.</summary>
        /// <param name="other"><see cref="Potion"/> to replace this instance</param>
        internal Potion(Potion other) : this(other.Name, other.HealAmount, other.Value, other.Hidden)
        {
        }

        #endregion Constructors
    }
}