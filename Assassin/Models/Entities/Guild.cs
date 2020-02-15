using System;
using System.Collections.Generic;
using System.Linq;

namespace Assassin.Models.Entities
{
    /// <summary>Represents a <see cref="Guild"/> that a <see cref="User"/> can join.</summary>
    public class Guild : BaseINPC
    {
        private int _id, _fee, _gold;
        private Henchmen _henchmen = new Henchmen(0, 0, 0, 0, 0);
        private string _name, _master;
        private List<string> _members = new List<string>();

        #region Modifying Properties

        /// <summary>ID of the <see cref="Guild"/> .</summary>
        public int ID
        {
            get => _id;
            set { _id = value; NotifyPropertyChanged(nameof(ID)); }
        }

        /// <summary>Name of the <see cref="Guild"/>.</summary>
        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(nameof(Name)); }
        }

        /// <summary>Master of the <see cref="Guild"/>.</summary>
        public string Master
        {
            get => _master;
            set { _master = value; NotifyPropertyChanged(nameof(Master)); }
        }

        /// <summary>Fee paid to join the <see cref="Guild"/>.</summary>
        public int Fee
        {
            get => _fee;
            set { _fee = value; NotifyPropertyChanged(nameof(Fee)); }
        }

        /// <summary>Gold owned by the <see cref="Guild"/>.</summary>
        public int Gold
        {
            get => _gold;
            set { _gold = value; NotifyPropertyChanged(nameof(Gold)); }
        }

        /// <summary>Members in the <see cref="Guild"/>.</summary>
        public List<string> Members
        {
            get => _members;
            set { _members = value; NotifyPropertyChanged(nameof(Members)); }
        }

        /// <summary>Amount of henchmen the <see cref="Guild"/> employs.</summary>
        public Henchmen Henchmen
        {
            get => _henchmen;
            set { _henchmen = value; NotifyPropertyChanged(nameof(Henchmen)); }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Fee paid to join the <see cref="Guild"/>, formatted.</summary>
        public string FeeToString => Fee.ToString("N0");

        /// <summary>Gold owned by the <see cref="Guild"/>, formatted.</summary>
        public string GoldToString => Gold.ToString("N0");

        #endregion Helper Properties

        /// <summary>Determines whether the <see cref="Guild "/> has a specific <see cref="User"/> as a member.</summary>
        /// <param name="member"><see cref="User"/> being checked</param>
        /// <returns>True if <see cref="User"/> is a member</returns>
        public bool HasMember(User member) => Members.Contains(member.Name);

        #region Override Operators

        public static bool Equals(Guild left, Guild right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return left.ID == right.ID && string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(left.Master, right.Master, StringComparison.OrdinalIgnoreCase) && left.Fee == right.Fee && left.Gold == right.Gold && left.Henchmen == right.Henchmen && !left.Members.Except(right.Members).Any() && !right.Members.Except(left.Members).Any();
        }

        public override bool Equals(object obj) => Equals(this, obj as Guild);

        public bool Equals(Guild other) => Equals(this, other);

        public static bool operator ==(Guild left, Guild right) => Equals(left, right);

        public static bool operator !=(Guild left, Guild right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => Name;

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Guild"/> class.</summary>
        public Guild()
        {
            ID = 0;
            Name = "";
            Master = "Computer";
            Fee = 50;
            Gold = 500;
            Members = new List<string>();
            Henchmen = new Henchmen(0, 0, 0, 0, 0);
        }

        /// <summary>Initializes a new instance of the <see cref="Guild"/> class using Property values.</summary>
        /// <param name="id">ID of <see cref="Guild"/></param>
        /// <param name="name">Name of <see cref="Guild"/></param>
        /// <param name="master">Name of Guildmaster</param>
        /// <param name="fee">Fee to enter <see cref="Guild"/></param>
        /// <param name="gold">Amount of Gold owned by <see cref="Guild"/></param>
        /// <param name="members">Names of <see cref="User"/>s who are a member of the <see cref="Guild"/></param>
        /// <param name="henchmen">Amount of <see cref="Henchmen"/> the <see cref="Guild"/> employs</param>
        public Guild(int id, string name, string master, int fee, int gold, List<string> members, Henchmen henchmen)
        {
            ID = id;
            Name = name;
            Master = master;
            Fee = fee;
            Gold = gold;
            Members = members;
            Henchmen = henchmen;
        }

        #endregion Constructors
    }
}