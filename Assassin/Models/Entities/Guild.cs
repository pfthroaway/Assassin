using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Assassin.Models.Entities
{
    /// <summary>Represents a <see cref="Guild"/> that a <see cref="User"/> can join.</summary>
    internal class Guild : INotifyPropertyChanged
    {
        private int _id, _fee, _gold, _henchmenLevel1, _henchmenLevel2, _henchmenLevel3, _henchmenLevel4, _henchmenLevel5;
        private string _name, _master;
        private List<string> _members = new List<string>();

        #region Properties

        /// <summary>ID of the <see cref="Guild"/> .</summary>
        public int ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged("ID"); }
        }

        /// <summary>Name of the <see cref="Guild"/>.</summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>Master of the <see cref="Guild"/>.</summary>
        public string Master
        {
            get => _master;
            set { _master = value; OnPropertyChanged("Master"); }
        }

        /// <summary>Fee paid to join the <see cref="Guild"/>.</summary>
        public int Fee
        {
            get => _fee;
            set { _fee = value; OnPropertyChanged("Fee"); }
        }

        /// <summary>Gold owned by the <see cref="Guild"/>.</summary>
        public int Gold
        {
            get => _gold;
            set { _gold = value; OnPropertyChanged("Gold"); }
        }

        /// <summary>Members in the <see cref="Guild"/>.</summary>
        public List<string> Members
        {
            get => _members;
            set { _members = value; OnPropertyChanged("Members"); }
        }

        /// <summary>Amount of level 1 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel1
        {
            get => _henchmenLevel1;
            set { _henchmenLevel1 = value; OnPropertyChanged("HenchmenLevel1"); }
        }

        /// <summary>Amount of level 2 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel2
        {
            get => _henchmenLevel2;
            set { _henchmenLevel2 = value; OnPropertyChanged("HenchmenLevel2"); }
        }

        /// <summary>Amount of level 3 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel3
        {
            get => _henchmenLevel3;
            set { _henchmenLevel3 = value; OnPropertyChanged("HenchmenLevel3"); }
        }

        /// <summary>Amount of level 4 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel4
        {
            get => _henchmenLevel4;
            set { _henchmenLevel4 = value; OnPropertyChanged("HenchmenLevel4"); }
        }

        /// <summary>Amount of level 5 henchmen hired by the <see cref="Guild"/>.</summary>
        public int HenchmenLevel5
        {
            get => _henchmenLevel5;
            set { _henchmenLevel5 = value; OnPropertyChanged("HenchmenLevel5"); }
        }

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Guild"/> class.</summary>
        internal Guild()
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
        internal Guild(int id, string name, string master, int fee, int gold, List<String> members, int henchmenLevel1, int henchmenLevel2, int henchmenLevel3, int henchmenLevel4, int henchmenLevel5)
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