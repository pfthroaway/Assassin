using Assassin.Models.Enums;
using System;

namespace Assassin.Models.Entities
{
    ///<summary>Represents a <see cref="User"/> who is in Jail.</summary>
    public class JailedUser : BaseINPC
    {
        private string _name;
        private Crime _reason;
        private int _fine;
        private DateTime _dateJailed;

        #region Modifying Properties

        /// <summary>Name of the <see cref="JailedUser"/>.</summary>
        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(nameof(Name)); }
        }

        /// <summary>Reason the <see cref="JailedUser"/> is in Jail.</summary>
        public Crime Reason
        {
            get => _reason;
            set { _reason = value; NotifyPropertyChanged(nameof(Reason)); }
        }

        /// <summary>Amount of gold required to release the <see cref="JailedUser"/> from Jail.</summary>
        public int Fine
        {
            get => _fine;
            set
            {
                _fine = value; NotifyPropertyChanged(nameof(Fine), nameof(FineToString));
            }
        }

        public string FineToString => Fine.ToString("N0");

        /// <summary>Date the <see cref="JailedUser"/> was incarcerated in UTC.</summary>
        public DateTime DateJailed
        {
            get => _dateJailed; set
            {
                _dateJailed = value;
                NotifyPropertyChanged(nameof(DateJailed), nameof(LocalDateJailed), nameof(LocalDateJailedToString));
            }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Date the <see cref="JailedUser"/> was incarcerated in local time.</summary>
        public DateTime LocalDateJailed => TimeZone.CurrentTimeZone.ToLocalTime(DateJailed);

        /// <summary>Date the <see cref="JailedUser"/> was incarcerated in local time, formatted.</summary>
        public string LocalDateJailedToString => LocalDateJailed.ToString(@"yyyy-MM-dd hh\:mm\:ss tt");

        #endregion Helper Properties

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="JailedUser"/>.</summary>
        public JailedUser()
        {
        }

        /// <summary>Initializes an instance of <see cref="JailedUser"/> by assigning Properties.</summary>
        /// <param name="name">Username</param>
        /// <param name="reason">Reason jailed</param>
        /// <param name="fine">Fine</param>
        /// <param name="dateJailed">Date the <see cref="JailedUser"/> was incarcerated in UTC</param>
        public JailedUser(string name, Crime reason, int fine, DateTime dateJailed)
        {
            Name = name;
            Reason = reason;
            Fine = fine;
            DateJailed = dateJailed;
        }

        /// <summary>Replaces this instance of <see cref="JailedUser"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="JailedUser"/> to replace this instance</param>
        public JailedUser(JailedUser other) : this(other.Name, other.Reason, other.Fine, other.DateJailed) { }

        #endregion Constructors
    }
}