using System;
using System.Collections.Generic;
using System.Linq;

namespace Assassin.Models.Entities
{
    /// <summary>Represents a <see cref="Guild"/> that a <see cref="User"/> can join.</summary>
    public class Guild : BaseINPC
    {
        private int _id, _fee, _gold, _henchmenLevel1, _henchmenLevel2, _henchmenLevel3, _henchmenLevel4, _henchmenLevel5;
        private string _name, _master;
        private List<string> _members = new List<string>();

        #region Properties

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

        /// <summary>Amount of level 1 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel1
        {
            get => _henchmenLevel1;
            set { _henchmenLevel1 = value; NotifyPropertyChanged(nameof(HenchmenLevel1)); }
        }

        /// <summary>Amount of level 2 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel2
        {
            get => _henchmenLevel2;
            set { _henchmenLevel2 = value; NotifyPropertyChanged(nameof(HenchmenLevel2)); }
        }

        /// <summary>Amount of level 3 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel3
        {
            get => _henchmenLevel3;
            set { _henchmenLevel3 = value; NotifyPropertyChanged(nameof(HenchmenLevel3)); }
        }

        /// <summary>Amount of level 4 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel4
        {
            get => _henchmenLevel4;
            set { _henchmenLevel4 = value; NotifyPropertyChanged(nameof(HenchmenLevel4)); }
        }

        /// <summary>Amount of level 5 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel5
        {
            get => _henchmenLevel5;
            set { _henchmenLevel5 = value; NotifyPropertyChanged(nameof(HenchmenLevel5)); }
        }

        #endregion Properties

        /// <summary>Determines whether the <see cref="Guild "/> has a specific <see cref="User"/> as a member.</summary>
        /// <param name="member"><see cref="User"/> being checked</param>
        /// <returns>True if <see cref="User"/> is a member</returns>
        public bool HasMember(User member) => Members.Contains(member.Name);

        #region Override Operators

        public static bool Equals(Guild left, Guild right)
        {
            if (left is null && right is null) return true;
            if (left is null ^ right is null) return false;
            return left.ID == right.ID && string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(left.Master, right.Master, StringComparison.OrdinalIgnoreCase) && left.Fee == right.Fee && left.Gold == right.Gold && left.HenchmenLevel1 == right.HenchmenLevel1 && left.HenchmenLevel2 == right.HenchmenLevel2 && left.HenchmenLevel3 == right.HenchmenLevel3 && left.HenchmenLevel4 == right.HenchmenLevel4 && left.HenchmenLevel5 == right.HenchmenLevel5 && !left.Members.Except(right.Members).Any() && !right.Members.Except(left.Members).Any();
        }

        public override bool Equals(object obj) => Equals(this, obj as Guild);

        public bool Equals(Guild other) => Equals(this, other);

        public static bool operator ==(Guild left, Guild right) => Equals(left, right);

        public static bool operator !=(Guild left, Guild right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

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
            HenchmenLevel1 = 0;
            HenchmenLevel2 = 0;
            HenchmenLevel3 = 0;
            HenchmenLevel4 = 0;
            HenchmenLevel5 = 0;
        }

        /// <summary>Initializes a new instance of the <see cref="Guild"/> class using Property values.</summary>
        /// <param name="id">ID of <see cref="Guild"/></param>
        /// <param name="name">Name of <see cref="Guild"/></param>
        /// <param name="master">Name of Guildmaster</param>
        /// <param name="fee">Fee to enter <see cref="Guild"/></param>
        /// <param name="gold">Amount of Gold owned by <see cref="Guild"/></param>
        /// <param name="members">Names of <see cref="User"/>s who are a member of the <see cref="Guild"/></param>
        /// <param name="henchmenLevel1">Amount of Level 1 Henchmen the <see cref="Guild"/> employs</param>
        /// <param name="henchmenLevel2">Amount of Level 2 Henchmen the <see cref="Guild"/> employs</param>
        /// <param name="henchmenLevel3">Amount of Level 3 Henchmen the <see cref="Guild"/> employs</param>
        /// <param name="henchmenLevel4">Amount of Level 4 Henchmen the <see cref="Guild"/> employs</param>
        /// <param name="henchmenLevel5">Amount of Level 5 Henchmen the <see cref="Guild"/> employs</param>
        public Guild(int id, string name, string master, int fee, int gold, List<String> members, int henchmenLevel1, int henchmenLevel2, int henchmenLevel3, int henchmenLevel4, int henchmenLevel5)
        {
            ID = id;
            Name = name;
            Master = master;
            Fee = fee;
            Gold = gold;
            Members = members;
            HenchmenLevel1 = henchmenLevel1;
            HenchmenLevel2 = henchmenLevel2;
            HenchmenLevel3 = henchmenLevel3;
            HenchmenLevel4 = henchmenLevel4;
            HenchmenLevel5 = henchmenLevel5;
        }

        #endregion Constructors
    }
}