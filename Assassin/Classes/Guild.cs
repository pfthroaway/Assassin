using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Assassin
{
    internal class Guild : INotifyPropertyChanged
    {
        private int _id, _fee, _gold, _henchmenLevel1, _henchmenLevel2, _henchmenLevel3, _henchmenLevel4, _henchmenLevel5;
        private string _name, _master;
        private List<string> _members = new List<string>();

        #region Properties

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public string Master
        {
            get { return _master; }
            set { _master = value; OnPropertyChanged("Master"); }
        }

        public int Fee
        {
            get { return _fee; }
            set { _fee = value; OnPropertyChanged("Fee"); }
        }

        public int Gold
        {
            get { return _gold; }
            set { _gold = value; OnPropertyChanged("Gold"); }
        }

        public List<string> Members
        {
            get { return _members; }
            set { _members = value; OnPropertyChanged("Members"); }
        }

        public int HenchmenLevel1
        {
            get { return _henchmenLevel1; }
            set { _henchmenLevel1 = value; OnPropertyChanged("HenchmenLevel1"); }
        }

        public int HenchmenLevel2
        {
            get { return _henchmenLevel2; }
            set { _henchmenLevel2 = value; OnPropertyChanged("HenchmenLevel2"); }
        }

        public int HenchmenLevel3
        {
            get { return _henchmenLevel3; }
            set { _henchmenLevel3 = value; OnPropertyChanged("HenchmenLevel3"); }
        }

        public int HenchmenLevel4
        {
            get { return _henchmenLevel4; }
            set { _henchmenLevel4 = value; OnPropertyChanged("HenchmenLevel4"); }
        }

        public int HenchmenLevel5
        {
            get { return _henchmenLevel5; }
            set { _henchmenLevel5 = value; OnPropertyChanged("HenchmenLevel5"); }
        }

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Guild class.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the Guild class using Property values.
        /// </summary>
        /// <param name="id">ID of Guild</param>
        /// <param name="name">Name of Guild</param>
        /// <param name="master">Name of Guildmaster</param>
        /// <param name="fee">Fee to enter Guild</param>
        /// <param name="gold">Amount of Gold owned by Guild</param>
        /// <param name="members">Names of Users who are a member of the Guild</param>
        /// <param name="henchmenLevel1">Amount of Level 1 Henchmen the Guild employs</param>
        /// <param name="henchmenLevel2">Amount of Level 2 Henchmen the Guild employs</param>
        /// <param name="henchmenLevel3">Amount of Level 3 Henchmen the Guild employs</param>
        /// <param name="henchmenLevel4">Amount of Level 4 Henchmen the Guild employs</param>
        /// <param name="henchmenLevel5">Amount of Level 5 Henchmen the Guild employs</param>
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