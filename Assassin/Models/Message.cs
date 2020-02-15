using System;

namespace Assassin.Models
{
    /// <summary>Represents a <see cref="Message"/> sent to a <see cref="User"/>.</summary>
    public class Message : BaseINPC
    {
        private int id;
        private string userFrom;
        private string userTo;
        private string contents;
        private DateTime dateSent;
        private bool guildMessage;

        #region Modifying Properties

        /// <summary>ID of the <see cref="Message"/>.</summary>
        public int ID
        {
            get => id;
            set { id = value; NotifyPropertyChanged(nameof(ID)); }
        }

        /// <summary><see cref="User"/> who sent the <see cref="Message"/>.</summary>
        public string UserFrom
        {
            get => userFrom;
            set { userFrom = value; NotifyPropertyChanged(nameof(UserFrom)); }
        }

        /// <summary><see cref="User"/> to whom the <see cref="Message"/> is written.</summary>
        public string UserTo
        {
            get => userTo;
            set { userTo = value; NotifyPropertyChanged(nameof(UserTo)); }
        }

        /// <summary>What the <see cref="Message"/> says.</summary>
        public string Contents
        {
            get => contents;
            set { contents = value; NotifyPropertyChanged(nameof(Contents)); }
        }

        /// <summary>Date the <see cref="Message"/> was sent in UTC.</summary>
        public DateTime DateSent
        {
            get => dateSent;
            set { dateSent = value; NotifyPropertyChanged(nameof(DateSent), nameof(LocalDateSent), nameof(LocalDateSentToString)); }
        }

        /// <summary>Was this <see cref="Message"/> sent by a guild leader?</summary>
        public bool GuildMessage
        {
            get => guildMessage;
            set { guildMessage = value; NotifyPropertyChanged(nameof(GuildMessage)); }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Date the <see cref="Message"/> was sent in local time.</summary>
        public DateTime LocalDateSent => TimeZone.CurrentTimeZone.ToLocalTime(DateSent);

        /// <summary>Date the <see cref="Message"/> was sent in local time, formatted.</summary>
        public string LocalDateSentToString => LocalDateSent.ToString(@"yyyy-MM-dd hh\:mm\:ss tt");

        #endregion Helper Properties

        /// <summary>Constructs a new instance of <see cref="Message"/> by assigning property values.</summary>
        /// <param name="sender"><see cref="User"/> who sent the <see cref="Message"/></param>
        /// <param name="recipient"><see cref="User"/> to whom the <see cref="Message"/> is written</param>
        /// <param name="contents">What the <see cref="Message"/> says</param>
        /// <param name="sent">Date the <see cref="Message"/> was sent</param>
        /// <param name="guildMsg">Was this <see cref="Message"/> sent by a guild leader?</param>
        public Message(int msgID, string sender, string recipient, string contents, DateTime sent, bool guildMsg)
        {
            ID = msgID;
            UserFrom = sender;
            UserTo = recipient;
            Contents = contents;
            DateSent = sent;
            GuildMessage = guildMsg;
        }

        /// <summary>Constructs a new instance of <see cref="Message"/> by copying another <see cref="Message"/>.</summary>
        /// <param name="other"><see cref="Message"/> to be copied</param>
        public Message(Message other)
        {
            ID = other.ID;
            UserFrom = other.UserFrom;
            UserTo = other.UserTo;
            Contents = other.Contents;
            DateSent = other.DateSent;
            GuildMessage = other.GuildMessage;
        }
    }
}