using Assassin.Models.Items;
using System;

namespace Assassin.Models.Entities
{
    /// <summary>Represents an <see cref="Enemy"/> a <see cref="User"/> fights.</summary>
    internal class Enemy : LivingEntity
    {
        private int _weaponSkill;
        private Weapon _weapon = new Weapon();

        #region Properties

        /// <summary><see cref="Enemy"/>'s Weapon.</summary>
        public Weapon Weapon
        {
            get => _weapon;
            set { _weapon = value; OnPropertyChanged("Weapon"); }
        }

        /// <summary><see cref="Enemy"/>'s skill with their Weapon.</summary>
        public int WeaponSkill
        {
            get => _weaponSkill;
            set { _weaponSkill = value; OnPropertyChanged("WeaponSkill"); }
        }

        #endregion Properties

        #region Override Operators

        public static bool Equals(Enemy left, Enemy right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && left.Level == right.Level && left.CurrentEndurance == right.CurrentEndurance && left.MaximumEndurance == right.MaximumEndurance && left.GoldOnHand == right.GoldOnHand && left.Armor == right.Armor && left.Weapon == right.Weapon && left.WeaponSkill == right.WeaponSkill;
        }

        public override bool Equals(object obj) => Equals(this, obj as Enemy);

        public bool Equals(Enemy other) => Equals(this, other);

        public static bool operator ==(Enemy left, Enemy right) => Equals(left, right);

        public static bool operator !=(Enemy left, Enemy right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        #region Health Manipulation

        /// <summary>The <see cref="Enemy"/> takes damage.</summary>
        /// <param name="damage">Amount of damage taken.</param>
        /// <returns>Message regarding damage taken</returns>
        public override string TakeDamage(int damage)
        {
            CurrentEndurance -= damage;
            return "The " + Name + " takes " + damage + " damage.";
        }

        #endregion Health Manipulation

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Enemy"/> class.</summary>
        internal Enemy()
        {
            Name = "";
            Level = 1;
            CurrentEndurance = 100;
            MaximumEndurance = 100;
            Weapon = new Weapon();
            Armor = new Armor();
            GoldOnHand = 100;
            WeaponSkill = 10;
            Blocking = 10;
            Slipping = 10;
        }

        /// <summary>Initializes a new instance of the <see cref="Enemy"/> class by assigning Property values.</summary>
        /// <param name="name">Name of <see cref="Enemy"/></param>
        /// <param name="level">Level of <see cref="Enemy"/></param>
        /// <param name="currentEndurance">Amount of Endurance the <see cref="Enemy"/> currently has</param>
        /// <param name="maximumEndurance">Maximum amount of Endurance the <see cref="Enemy"/> can have</param>
        /// <param name="weapon">Weapon equipped by the <see cref="Enemy"/></param>
        /// <param name="armor">Armor equipped by the <see cref="Enemy"/></param>
        /// <param name="goldOnHand">Amount of Gold the <see cref="Enemy"/> is currently carrying</param>
        /// <param name="weaponSkill">Amount of skill the <see cref="Enemy"/> has with their Weapon</param>
        /// <param name="blocking">Amount of skill the <see cref="Enemy"/> has with blocking incoming attacks</param>
        /// <param name="slipping">Amount of skill the <see cref="Enemy"/> has with dodging attacks and fleeing battles</param>
        internal Enemy(string name, int level, int currentEndurance, int maximumEndurance, Weapon weapon, Armor armor, int goldOnHand, int weaponSkill, int blocking, int slipping)
        {
            Name = name;
            Level = level;
            CurrentEndurance = currentEndurance;
            MaximumEndurance = maximumEndurance;
            Weapon = weapon;
            Armor = armor;
            GoldOnHand = goldOnHand;
            WeaponSkill = weaponSkill;
            Blocking = blocking;
            Slipping = slipping;
        }

        /// <summary>Replaces this instance of Enemy with another instance.</summary>
        /// <param name="other">Enemy to replace this instance</param>
        internal Enemy(Enemy other) : this(other.Name, other.Level, other.CurrentEndurance, other.MaximumEndurance, other.Weapon, other.Armor, other.GoldOnHand, other.WeaponSkill, other.Blocking, other.Slipping)
        {
        }

        #endregion Constructors
    }
}