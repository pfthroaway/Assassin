namespace Assassin.Models.Entities
{
    /// <summary>Represents the amount of Henchmen hired by the Entity.</summary>
    public class Henchmen : BaseINPC
    {
        private int _level1, _level2, _level3, _level4, _level5;

        /// <summary>Amount of level 1 henchmen hired by the Entity.</summary>
        public int Level1
        {
            get => _level1;
            set { _level1 = value; NotifyPropertyChanged(nameof(Level1)); }
        }

        /// <summary>Amount of level 2 henchmen hired by the Entity.</summary>
        public int Level2
        {
            get => _level2;
            set { _level2 = value; NotifyPropertyChanged(nameof(Level2)); }
        }

        /// <summary>Amount of level 3 henchmen hired by the Entity.</summary>
        public int Level3
        {
            get => _level3;
            set { _level3 = value; NotifyPropertyChanged(nameof(Level3)); }
        }

        /// <summary>Amount of level 4 henchmen hired by the Entity.</summary>
        public int Level4
        {
            get => _level4;
            set { _level4 = value; NotifyPropertyChanged(nameof(Level4)); }
        }

        /// <summary>Amount of level 5 henchmen hired by the Entity.</summary>
        public int Level5
        {
            get => _level5;
            set { _level5 = value; NotifyPropertyChanged(nameof(Level5)); }
        }

        #region Override Operators

        public static bool Equals(Henchmen left, Henchmen right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return left.Level1 == right.Level1 && left.Level2 == right.Level2 && left.Level3 == right.Level3 && left.Level4 == right.Level4 && left.Level5 == right.Level5;
        }

        public override bool Equals(object obj) => Equals(this, obj as Henchmen);

        public bool Equals(Henchmen other) => Equals(this, other);

        public static bool operator ==(Henchmen left, Henchmen right) => Equals(left, right);

        public static bool operator !=(Henchmen left, Henchmen right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        #endregion Override Operators

        /// <summary>Initializes an instance of <see cref="Henchmen"/> by assigning values to Properties.</summary>
        /// <param name="level1">Amount of level 1 Henchmen the Entity employs</param>
        /// <param name="level2">Amount of level 2 Henchmen the Entity employs</param>
        /// <param name="level3">Amount of level 3 Henchmen the Entity employs</param>
        /// <param name="level4">Amount of level 4 Henchmen the Entity employs</param>
        /// <param name="level5">Amount of Level 5 Henchmen the Entity employs</param>
        public Henchmen(int level1, int level2, int level3, int level4, int level5)
        {
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            Level4 = level4;
            Level5 = level5;
        }

        /// <summary>Replaces this instance of <see cref="Henchmen"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="Henchmen"/> to replace this instance</param>
        public Henchmen(Henchmen other) : this(other.Level1, other.Level2, other.Level3, other.Level4, other.Level5)
        {
        }
    }
}