using System.Collections.Generic;
using System.ComponentModel;

namespace Assassin
{
    internal class Jail : INotifyPropertyChanged
    {
        private List<JailedUser> _jailedList;

        internal List<JailedUser> JailedList
        {
            get { return _jailedList; }
            set { _jailedList = value; OnPropertyChanged("JailedList"); }
        }

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Constructors

        /// <summary>
        /// Initializes a default instance of Jail.
        /// </summary>
        public Jail()
        {
        }

        /// <summary>
        /// Initializes an instance of Jail by assigning the list of jailed Users.
        /// </summary>
        /// <param name="jailedList">List of jailed Users.</param>
        public Jail(List<JailedUser> jailedList)
        {
            JailedList = jailedList;
        }

        #endregion Constructors
    }
}