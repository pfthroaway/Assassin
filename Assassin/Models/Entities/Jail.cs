using System.Collections.Generic;
using System.ComponentModel;

namespace Assassin.Models.Entities
{
    /// <summary>Represents the <see cref="Jail"/>, where all <see cref="User"/>s who are arrested are taken.</summary>
    internal class Jail : INotifyPropertyChanged
    {
        private List<JailedUser> _jailedList;

        /// <summary>List of <see cref="User"/>s currently in <see cref="Jail"/>.</summary>
        internal List<JailedUser> JailedList
        {
            get => _jailedList;
            set { _jailedList = value; OnPropertyChanged("JailedList"); }
        }

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        #region Constructors

        /// <summary>Initializes a default instance of Jail.</summary>
        public Jail()
        {
        }

        /// <summary>Initializes an instance of <see cref="Jail"/> by assigning the list of jailed <see cref="User"/>s.</summary>
        /// <param name="jailedList">List of jailed <see cref="User"/>s.</param>
        public Jail(List<JailedUser> jailedList) => JailedList = jailedList;

        #endregion Constructors
    }
}