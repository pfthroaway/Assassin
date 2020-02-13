using System.ComponentModel;

namespace Assassin.Models.Entities
{
    internal class JailedUser : INotifyPropertyChanged
    {
        private string _username, _reason;
        private int _fine;

        #region Properties

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged("Username"); }
        }

        public string Reason
        {
            get => _reason;
            set { _reason = value; OnPropertyChanged("Reason"); }
        }

        public int Fine
        {
            get => _fine;
            set { _fine = value; OnPropertyChanged("Fine"); OnPropertyChanged("FineToString"); }
        }

        public string FineToString => Fine.ToString("N0");

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Data-Binding

        #region Constructors

        /// <summary>
        /// Initializes a default instance of JailedUser.
        /// </summary>
        public JailedUser()
        {
        }

        /// <summary>
        /// Initializes an instance of JailedUser by assigning Properties.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="reason">Reason jailed</param>
        /// <param name="fine">Fine</param>
        public JailedUser(string username, string reason, int fine)
        {
            Username = username;
            Reason = reason;
            Fine = fine;
        }

        #endregion Constructors
    }
}