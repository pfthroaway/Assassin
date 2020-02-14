using System.Collections.Generic;

namespace Assassin.Models.Entities
{
    /// <summary>Represents the <see cref="Jail"/>, where all <see cref="User"/>s who are arrested are taken.</summary>
    internal class Jail : BaseINPC
    {
        private List<JailedUser> _jailedList;

        /// <summary>List of <see cref="User"/>s currently in <see cref="Jail"/>.</summary>
        internal List<JailedUser> JailedList
        {
            get => _jailedList;
            set { _jailedList = value; NotifyPropertyChanged(nameof(JailedList)); }
        }

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